using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(JsonSaving))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private Transform cellsParent;
    [Space(10)]
    [SerializeField] private Square squarePrefab;
    [SerializeField] private Transform squarePrefabTransform;
    [SerializeField] private Transform squarePrefabParent;

    [SerializeField] private List<Square> squares = new List<Square>();
    [SerializeField] private Square newSquare = new Square();
    [SerializeField] private Square smallSquare = new Square();

    private Cell[,] cells;
    private Square[,] map;
    private const int SIZE_MAP = 3;
    private const int MAX_AMOUNT_BALLS_DELETE = 3;
    private bool[,] mark;
    private bool isWin = false;

    public int sizeMap => SIZE_MAP;
    public int SquaresCount => squares.Count;

    private JsonSaving jsonSaving;

    private void Awake()
    {
        Instance = this;
        map = new Square[SIZE_MAP, SIZE_MAP];
        cells = new Cell[SIZE_MAP, SIZE_MAP];

        jsonSaving = GetComponent<JsonSaving>();
    }

    private void Start()
    {
        PanelsController.Instance.EnabledPanel(0);
    }

    public void StartGame()
    {
        AddCells();
        Load();

        if (newSquare == null)
            InstantiateSquare();
    }

    private void Load()
    {
        SquaresSaving squaresSaving = jsonSaving.Load();

        if (squaresSaving.isSave)
            for (int x = 0; x < SIZE_MAP; x++)
                for (int y = 0; y < SIZE_MAP; y++)
                    if (squaresSaving.SquaresSavings[x, y] != null)
                        SetMap(InstantiateSquareLoad(squaresSaving.SquaresSavings[x, y]), x, y);

        if (squaresSaving.isSave && squaresSaving.NewSquare.isSave) AddNewSquare(InstantiateSquareLoad(squaresSaving.NewSquare));
        if (squaresSaving.isSave && squaresSaving.SmallSquare.isSave) AddSmallSquare(InstantiateSquareLoad(squaresSaving.SmallSquare));
    }


    private void AddCells()
    {
        int numCell = 0;
        for (int x = 0; x < SIZE_MAP; x++)
            for (int y = 0; y < SIZE_MAP; y++)
            {
                Cell cell = cellsParent.GetChild(numCell).GetComponent<Cell>();
                cells[x, y] = cell;
                numCell++;
            }
    }

    public void InstantiateSquare()
    {
        Square newSquare = Instantiate(squarePrefab, squarePrefabTransform.position, Quaternion.identity, squarePrefabParent);
        newSquare.SetColor(SetSquareColor());
        newSquare.AddVector3Map(squarePrefabTransform.position);
        squares.Add(newSquare);
        AddNewSquare(newSquare);
    }

    public Square InstantiateSquareLoad(SquareSaving loadSquare)
    {
        Square newSquare = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity, squarePrefabParent);
        newSquare.RectTransformSquare.position = loadSquare.Vector3Map;
        newSquare.SetColor(loadSquare.SquareColor);
        newSquare.AddSaving(loadSquare);
        squares.Add(newSquare);

        return newSquare;
    }

    private SquareColor SetSquareColor()
    {
        int random = Random.Range(1, 4);
        SquareColor squareColor;

        switch (random)
        {
            case 1:
                squareColor = SquareColor.Green;
                break;
            case 2:
                squareColor = SquareColor.Blue;
                break;
            case 3:
                squareColor = SquareColor.Red;
                break;
            default:
                squareColor = SquareColor.Green;
                break;
        }

        return squareColor;
    }

    public void CutLines()
    {
        int countSquares = 0;
        mark = new bool[SIZE_MAP, SIZE_MAP];

        for (int x = 0; x < SIZE_MAP; x++)
            for (int y = 0; y < SIZE_MAP; y++)
            {
                countSquares += CutLine(x, y, 1, 0);
                countSquares += CutLine(x, y, 0, 1);
                countSquares += CutLine(x, y, 1, 1);
                countSquares += CutLine(x, y, -1, 1);
            }

        if (countSquares > 0)
        {
            Counter.IncreaseCounter.Invoke(countSquares);

            for (int x = 0; x < SIZE_MAP; x++)
                for (int y = 0; y < SIZE_MAP; y++)
                    if (mark[x, y])
                    {
                        map[x, y].DestroySquare();
                        map[x, y] = null;
                        cells[x, y].EnableTakeCoordinates(true);
                        squares.Remove(map[x, y]);
                    }
        }

        if (IsGameOver())
        {
            jsonSaving.Clear();
            Counter.GameOver.Invoke();
            StartCoroutine(PanelDelay(4));
            StartCoroutine(RemoveAllSquare(.4f));
        }
        else if(!isWin)
        {
            jsonSaving.Save(map, newSquare, smallSquare);
        }
        else if(isWin && !IsGameOver())
        {
            jsonSaving.Clear();
            Counter.GameOver.Invoke();
            StartCoroutine(PanelDelay(3));
            StartCoroutine(RemoveAllSquare(.3f));
        }
    }

    private IEnumerator PanelDelay(int numPanel)
    {
        yield return new WaitForSeconds(.5f);
        PanelsController.Instance.EnabledPanel(numPanel);
    }

    private IEnumerator RemoveAllSquare(float second)
    {
        yield return new WaitForSeconds(second);
        Debug.Log(squares.Count);
        for (int i = 0; i < squares.Count; i++)
            if (squares[i] != null)
                squares[i].DestroySquare();
    }

    private int CutLine(int x0, int y0, int sx, int sy)
    {
        Square colorIndex = map[x0, y0];

        if (colorIndex == null) return 0;

        int count = 0;

        for (int x = x0, y = y0; GetMap(x, y).squareColor == colorIndex.squareColor; x += sx, y += sy)
            count++;

        if (count < MAX_AMOUNT_BALLS_DELETE)
            return 0;

        for (int x = x0, y = y0; GetMap(x, y).squareColor == colorIndex.squareColor; x += sx, y += sy)
            mark[x, y] = true;

        return count;
    }

    private bool OnMap(int x, int y)
    {
        return x >= 0 && x < SIZE_MAP &&
               y >= 0 && y < SIZE_MAP;
    }

    private Square GetMap(int x, int y)
    {
        if (!OnMap(x, y) || map[x, y] == null)
        {
            Square square = new Square();
            square.SetColor(SquareColor.Empty);

            return square;
        }

        return map[x, y];
    }

    public void SetMap(Square square, int number)
    {
        int nr = number;
        int x = nr / SIZE_MAP;
        int y = nr % SIZE_MAP;

        map[x, y] = square;
        cells[x, y].EnableTakeCoordinates(false);
    }

    public void SetMap(Square square, int x, int y)
    {
        map[x, y] = square;
        cells[x, y].EnableTakeCoordinates(false);
    }

    public void AddNewSquare(Square square) => newSquare = square;
    public void RemoveNewSquare() => newSquare = null;
    public void AddSmallSquare(Square square)
    {
        smallSquare = square;
        jsonSaving.Save(map, newSquare, smallSquare);
    }
    public void RemoveSmallSquare() => smallSquare = null;

    public void Win() => isWin = true;
    private bool IsGameOver()
    {
        for (int x = 0; x < SIZE_MAP; x++)
            for (int y = 0; y < SIZE_MAP; y++)
                if (map[x, y] == null)
                    return false;

        return true;
    }
}

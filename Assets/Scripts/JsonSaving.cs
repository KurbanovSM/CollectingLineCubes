using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonSaving : MonoBehaviour
{
    private string path = "";

    private void Awake()
    {
        CheckPlatfom();
        if(PlayerPrefs.GetInt("FirstLaunch", 0) == 0)
        {
            Clear();
            PlayerPrefs.SetInt("FirstLaunch", 1);
        }
    }

    public void Save(Square[,] map, Square newSquare, Square smallSquare)
    {
        int size = GameController.Instance.sizeMap;

        SquaresSaving squaresSaving = new SquaresSaving(true);

        if (newSquare != null) squaresSaving.NewSquare = new SquareSaving(newSquare);
        if (smallSquare != null) squaresSaving.SmallSquare = new SquareSaving(smallSquare);

        squaresSaving.SquaresSavings = ConversionToSave(map, size);

        File.WriteAllText(path, JsonConvert.SerializeObject(squaresSaving, Formatting.Indented,
        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        Debug.Log("Save");
    }

    private SquareSaving[,] ConversionToSave(Square[,] map, int size)
    {
        SquareSaving[,] squareSavings = new SquareSaving[size, size];

        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                if (map[x, y] != null)
                {
                    SquareSaving squareSaving = new SquareSaving(map[x, y]);
                    squareSavings[x, y] = squareSaving;
                }

        return squareSavings;
    }

    public SquaresSaving Load()
    {
        SquaresSaving squaresSaving = JsonConvert.DeserializeObject<SquaresSaving>(File.ReadAllText(path));

        Debug.Log("Load");

        return squaresSaving;
    }

    public void Clear()
    {
        SquaresSaving squaresSaving = new SquaresSaving();

        File.WriteAllText(path,
       JsonConvert.SerializeObject(squaresSaving, Formatting.Indented, new JsonSerializerSettings()
       { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        Debug.Log("Clear");
    }

    private void CheckPlatfom()
    {
#if UNITY_EDITOR
        path = Application.dataPath + "/ProressSaving.json";
#else
        path = Application.persistentDataPath + "/ProressSaving.json";
#endif
    }
}

[System.Serializable]
public class SquaresSaving
{
    public bool isSave = false;
    public SquareSaving[,] SquaresSavings = new SquareSaving[GameController.Instance.sizeMap, GameController.Instance.sizeMap];
    public SquareSaving NewSquare = new SquareSaving();
    public SquareSaving SmallSquare = new SquareSaving();

    public SquaresSaving(bool isSave = false)
    {
        this.isSave = isSave;
    }
}

[System.Serializable]
public class SquareSaving
{
    public bool isSave = false;
    public bool IsMove = false;
    public bool IsWasOnSmallCell = false;
    public SquareColor SquareColor = SquareColor.Empty;
    public Vector3 Vector3Map = Vector3.zero;

    public SquareSaving() { }

    public SquareSaving(Square square)
    {
        IsMove = square.isMove;
        IsWasOnSmallCell = square.isWasOnSmallCell;
        Vector3Map = square.vector3Map;
        SquareColor = square.squareColor;

        isSave = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonSaving : MonoBehaviour
{
    private string path = "";

    private void Awake()
    {
        CheckPlatfom();
        if(PlayerPrefs.GetInt("OnePlay", 0) == 0)
        {
            Clear();
            PlayerPrefs.SetInt("OnePlay", 1);
        }
    }

    public void Save(Square[,] map, Square newSquare, Square smallSquare)
    {
        int size = GameController.Instance.sizeMap;

        SquaresSaving squaresSaving = new SquaresSaving();

        squaresSaving.isSave = true;

        if (newSquare != null)
        {
            squaresSaving.NewSquare = new SquareSaving(newSquare);
            squaresSaving.NewSquare.isSave = true;
        }
        if (smallSquare != null)
        {
            squaresSaving.SmallSquare = new SquareSaving(smallSquare);
            squaresSaving.SmallSquare.isSave = true;
        }

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
}

[System.Serializable]
public class SquareSaving
{
    public bool isSave = false;
    public bool IsMove = false;
    public bool IsWasOnSmallCell = false;
    public SquareColor SquareColor = SquareColor.Empty;
    public Vector2 Vector2Map = Vector2.zero;

    public SquareSaving() { }

    public SquareSaving(Square square)
    {
        IsMove = square.isMove;
        IsWasOnSmallCell = square.isWasOnSmallCell;
        Vector2Map = square.RectTransformSquare.localPosition;
        SquareColor = square.squareColor;
    }
}

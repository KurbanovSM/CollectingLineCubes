using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Square : MonoBehaviour
{
    public bool isMove { get; private set; } = true;
    public bool isWasOnSmallCell { get; private set; } = false;
    public RectTransform RectTransformSquare { get; private set; }
    public SquareColor squareColor { get; private set; }
    public Vector3 vector3Map { get; private set; }

    private CanvasGroup canvasGroup;

    [SerializeField] private float speedMove;

    private void Awake()
    {
        RectTransformSquare = transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetColor(SquareColor squareColor)
    {
        this.squareColor = squareColor;

        switch (this.squareColor)
        {
            case SquareColor.Empty:
                break;
            case SquareColor.Green:
                GetComponent<Image>().color = Color.green;
                break;
            case SquareColor.Blue:
                GetComponent<Image>().color = Color.blue;
                break;
            case SquareColor.Red:
                GetComponent<Image>().color = Color.red;
                break;
        }
    }

    public void SmoothDampSetPosition(Vector3 newVector)
    {
        if(newVector == Vector3.zero)
        {
            StartCoroutine(SmoothDamPositionCorutine(vector3Map));
        }
        else
        {
            StartCoroutine(SmoothDamPositionCorutine(newVector));
            AddVector3Map(newVector);
        }
    }

    public void SetCanvasGroup(bool enabled)
    {
        canvasGroup.blocksRaycasts = enabled;
    }

    public void SetPotitionAndLocalPositionOffset(Vector3 newVector, Vector3 offset)
    {
        RectTransformSquare.position = newVector;
        RectTransformSquare.localPosition += offset;
    }

    public void DisableMove()
    {
        isMove = false;
    }

    private IEnumerator SmoothDamPositionCorutine(Vector3 newVector)
    {
        while (RectTransformSquare.position != newVector)
        {
            RectTransformSquare.position = Vector3.MoveTowards(RectTransformSquare.position, newVector, speedMove * Time.deltaTime);
            yield return null;
        }
    }

    public void EnableWasOnSmallCell(bool enable)
    {
        isWasOnSmallCell = enable;
    }

    public void DestroySquare()
    {
        SetCanvasGroup(false);
        StartCoroutine(ChangeScale());
    }

    private IEnumerator ChangeScale()
    {
        while (RectTransformSquare.localScale != Vector3.zero)
        {
            RectTransformSquare.rotation *= Quaternion.Euler(0,0,10);
            RectTransformSquare.localScale = Vector3.MoveTowards(RectTransformSquare.localScale, Vector3.zero, speedMove * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    public void AddSaving(SquareSaving squareSavings)
    {
        isMove = squareSavings.IsMove;
        isWasOnSmallCell = squareSavings.IsWasOnSmallCell;
    }

    public void AddVector3Map(Vector3 newVector)
    {
        vector3Map = newVector;
    }
}

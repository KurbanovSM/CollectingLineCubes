using UnityEngine;
using UnityEngine.EventSystems;

public class SquareMove : MonoBehaviour, IDragHandler, IPointerDownHandler, IDropHandler
{
    [SerializeField] private Vector3 offset;
    private Square square = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out Square square) && square.isMove)
        {
            square.SetCanvasGroup(false);
            this.square = square;
            square.transform.SetSiblingIndex(GameController.Instance.SquaresCount - 1);
            SetPosition(eventData);

            ClickSound.Click.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData);
    }

    private void SetPosition(PointerEventData eventData)
    {
        if (square != null && RectTransformUtility.ScreenPointToWorldPointInRectangle
           (square.RectTransformSquare, eventData.position, eventData.pressEventCamera, out var globalMousePosition))
        {
            square.SetPotitionAndLocalPositionOffset(globalMousePosition, offset);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (square == null) return;

        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out SmallCell smallCell) && smallCell.IsTakeCoordinates)
        {
            ClickSound.Click.Invoke();
            smallCell.EnableTakeCoordinates(false);
            RectTransform smallCellRectTransform = smallCell.gameObject.transform as RectTransform;
            square.SmoothDampSetPosition(smallCellRectTransform.position);
            square.AddVector3Map(smallCellRectTransform.position);
            square.EnableWasOnSmallCell(true);
            GameController.Instance.InstantiateSquare();
            GameController.Instance.AddSmallSquare(square);
        }
        else if(eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out Cell cell) && cell.IsTakeCoordinates)
        {
            ClickSound.Click.Invoke();
            RectTransform cellRectTransform = cell.gameObject.transform as RectTransform;
            square.SmoothDampSetPosition(cellRectTransform.position);
            square.DisableMove();

            if (!square.isWasOnSmallCell)
            {
                GameController.Instance.RemoveNewSquare();
                GameController.Instance.InstantiateSquare();
            }
            else
            {
               square.EnableWasOnSmallCell(false);
               GameController.Instance.RemoveSmallSquare();
               SmallCell.EnableTakeCoordinates.Invoke();
            }

            GameController.Instance.SetMap(square, cell.transform.GetSiblingIndex());
            GameController.Instance.CutLines();
        }
        else
        {
            square.SmoothDampSetPosition(Vector3.zero);
        }

        DestroySquare();
    }

    private void DestroySquare()
    {
        square.SetCanvasGroup(true);
        square = null;
    }
}

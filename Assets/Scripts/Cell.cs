using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsTakeCoordinates = true;
    public void EnableTakeCoordinates(bool enable)
    {
        IsTakeCoordinates = enable;
    }
}

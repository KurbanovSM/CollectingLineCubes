using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public bool IsTakeCoordinates = true;
    public void EnableTakeCoordinates(bool enable)
    {
        IsTakeCoordinates = enable;
    }
}

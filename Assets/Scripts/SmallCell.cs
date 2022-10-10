using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmallCell : Cell
{
    public static UnityEvent EnableTakeCoordinates = new UnityEvent();

    private void Awake()
    {
        EnableTakeCoordinates.AddListener(EnableTakeCoordinatesMethod);
    }

    private void EnableTakeCoordinatesMethod() => EnableTakeCoordinates(true);
}

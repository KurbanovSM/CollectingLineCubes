using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsController : MonoBehaviour
{
    public static PanelsController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private List<Panel> panels;

    public void EnabledPanel(int numPanel)
    {
        panels[numPanel].EnabledPanel();
        ClickSound.Click.Invoke();
    }

    public void DisabledPanel(int numPanel)
    {
        panels[numPanel].DisabledPanel();
        ClickSound.Click.Invoke();
    }
}

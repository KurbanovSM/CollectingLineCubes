using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Counter : MonoBehaviour
{
    public static UnityEvent<int> IncreaseCounter = new UnityEvent<int>();
    public static UnityEvent GameOver = new UnityEvent();

    [SerializeField] private int maxCounter;
    [SerializeField] private TMP_Text counterText;
    private int counter { get => PlayerPrefs.GetInt("counter", 0); set => PlayerPrefs.SetInt("counter", value); }

    private void Awake()
    {
        IncreaseCounter.AddListener(Add);
        GameOver.AddListener(ClearCount);
        UpdateUI();
    }

    private void Add(int count)
    {
        counter += count;

        UpdateUI();
        
        if (counter >= maxCounter)
        {
            GameController.Instance.Win();
            ClearCount();
        }
    }

    private void UpdateUI()
    {
        counterText.text = $"{counter}/{maxCounter}";
    }
    private void ClearCount()
    {
        counter = 0;
    }
}

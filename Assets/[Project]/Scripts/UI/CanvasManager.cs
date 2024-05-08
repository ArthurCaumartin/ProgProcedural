using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private TextMeshProUGUI _coinText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetCoinText(CoinSystem.instance.coin);
    }

    public void SetCoinText(int value)
    {
        _coinText.text = "Coins : " + value.ToString();
    }
}

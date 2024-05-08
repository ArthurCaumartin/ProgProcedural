using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public static CoinSystem instance;

    void Awake()
    {
        instance = this;
    }

    public bool canbuy = false;
    public int coin = 10;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            CanvasManager.instance.SetCoinText(coin);
        }
    }
    
    public void cost()
    {
        Coin -= 10;
        canbuy = false;
    }
    public void CheckCount()
    {
        if (Coin < 10)
        {
            print("t pauvre");
        }
        else
        {
            canbuy = true;
        }
    }
}

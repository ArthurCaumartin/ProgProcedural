using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public int coin = 10;
    public void cost()
    {
        coin =- 10;
    }
    public void CheckCount()
    {
        if(coin <=10)
        {
            print("no");
        } 
        else
        {
            cost();
        }
    }
}

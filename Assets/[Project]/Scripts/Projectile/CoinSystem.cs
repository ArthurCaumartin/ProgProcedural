using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public bool canbuy = false;
    public int coin = 10;
    public void cost()
    {
        coin -= 10;
        canbuy = false;
    }
    public void CheckCount()
    {
        if(coin <10)
        {
            print("t pauvre");
        } 
        else
        {
            canbuy = true;
        }
    }
    private void Update() 
    {
        
    }
}

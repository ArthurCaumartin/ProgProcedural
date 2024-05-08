using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : MonoBehaviour
{
    public GameObject Shop;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Shop.SetActive(true);
        }
    }
}


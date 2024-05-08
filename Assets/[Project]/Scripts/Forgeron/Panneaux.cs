using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Panneaux : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            SpawnPanneaux(); 
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            transform.DOMoveY(-0.7f, 1f);    
        }
    }
    public void SpawnPanneaux()
    {
        transform.DOMoveY(1.5f, 1f);    
    }
}

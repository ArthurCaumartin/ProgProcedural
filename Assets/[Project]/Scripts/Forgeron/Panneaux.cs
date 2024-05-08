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
            print("enter");
            Debug.DrawRay(other.transform.position, Vector3.up, Color.white, 3f);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            transform.DOMoveY(-0.7f, 1f);    
            print("Exit");
        }
    }
    public void SpawnPanneaux()
    {
        transform.DOMoveY(1.5f, 1f);    
    }
}

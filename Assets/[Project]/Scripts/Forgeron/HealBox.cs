using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour
{
    public PlayerHealth playerHealth;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            BuffHeal();
        }
    }
    public void BuffHeal()
    {
        playerHealth.health += 5;
    }
}

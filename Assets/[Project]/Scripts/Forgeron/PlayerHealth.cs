using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    private int maxHealth = 150;
    private void Update() 
    {
        if (health <= 0)
        {
            print("you lose");
        }
        if(health > maxHealth)
        {
            health = maxHealth;
            print("health nerf");
        }
    }
}

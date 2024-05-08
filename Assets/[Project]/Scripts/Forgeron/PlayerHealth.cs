using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    private int maxHealth = 150;
    private void Update() 
    {
        if (health <= 0)
        {
            print("you lose");
            SceneManager.LoadScene("Menu");
        }
        if(health > maxHealth)
        {
            health = maxHealth;
            print("health nerf");
        }
    }
}

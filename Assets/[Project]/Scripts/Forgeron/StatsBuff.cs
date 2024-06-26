using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StatsBuff : MonoBehaviour
{
    public ProjectileShooter projectileShooter;
    public HealBox healBox;
    public CharacterMovement characterMovement;
    public CoinSystem coinSystem;
    public MineGenerator mineGenerator;
    public void HealUp()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            healBox.BuffHeal();
        }
    }
    public void SpeedUp()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            characterMovement.AddSpeed();
        }
    }
    public void UnlockShooter()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            projectileShooter.UnlockShot();
        }
    }
    public void AS()

    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            projectileShooter.BuffAs();
        }
    }
    public void Pierce()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            projectileShooter.BuffPierce();
        }
    }
    public void ProjectileSpeedBuff()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            projectileShooter.ProjectileSpeed();
        }
    }
    public void Damage()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            projectileShooter.BuffDamage();
        }
    }
    public void Unlockdash()
    {
        if(coinSystem.canbuy == true)
        {
            coinSystem.cost();
            characterMovement.DashUnlock();
        }
    }
    public void DevilDeal()
    {
        coinSystem.cost();
        mineGenerator.SetEnemySpawn(true);
    }
    public void AoeBuff()
    {
        coinSystem.cost();
        projectileShooter._isAoeEnable = true;
    }
}

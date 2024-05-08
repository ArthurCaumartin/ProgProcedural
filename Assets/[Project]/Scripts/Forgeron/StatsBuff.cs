using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBuff : MonoBehaviour
{
    public ProjectileShooter projectileShooter;
    public HealBox healBox;
    public CharacterMovement speedBuff;
    public void HealUp()
    {
        healBox.BuffHeal();
    }
    public void SpeedUp()
    {
        speedBuff.AddSpeed();
    }
    public void UnlockShooter()
    {
        projectileShooter.UnlockShot();
    }
    public void AS()

    {
        projectileShooter.BuffAs();
    }
    public void Pierce()
    {
        projectileShooter.BuffPierce();
    }
    public void ProjectileSpeedBuff()
    {
        projectileShooter.ProjectileSpeed();
    }
    public void Damage()
    {
        projectileShooter.BuffDamage();
    }
}

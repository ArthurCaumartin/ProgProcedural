using System;
using UnityEngine;

[Serializable]
public enum CellType
{
    Dirt,
    Stone,
    HardStone,
    Mineral,
    Wall,
}

public class TerrainCell : MonoBehaviour
{
    [SerializeField] private CellType _type;
    [SerializeField] private float _lifePoint = 2;

    public CellType Type {get => _type;}

    public void OnHit(int damageDeal, Projectile projectileHit)
    {
        if(projectileHit && _type == CellType.Wall)
            Destroy(projectileHit.gameObject);

        _lifePoint -= damageDeal;
        if (_lifePoint <= 0)
        {
            // if (_type == CellType.Mineral)
                //! Add Coin / Exp 

            Destroy(gameObject);
        }
    }
}

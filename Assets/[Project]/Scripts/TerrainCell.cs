using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
enum CellType
{
    Dirt,
    Stone,
    Mineral,
}

public class TerrainCell : MonoBehaviour
{
    [SerializeField] private CellType _type;
    [SerializeField] private float _lifePoint = 2;

    public void OnHit()
    {
        _lifePoint--;
        if (_lifePoint <= 0)
        {
            // if (_type == CellType.Mineral)
                //! Add Coin / Exp 

            Destroy(gameObject);
        }
    }
}

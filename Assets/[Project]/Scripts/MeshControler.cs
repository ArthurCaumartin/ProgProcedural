using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeshControler : MonoBehaviour
{
    [SerializeField] Transform _target;
    // [SerializeField] private SecondOrder<Vector3> _data2;

    void Update()
    {
        transform.position = _target.position;
    }
}

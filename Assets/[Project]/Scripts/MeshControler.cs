using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeshControler : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] private SecondOrder<Vector3> _data2;
    // [SerializeField] private Se _data;

    void Update()
    {
        transform.position = SecondOrderDynamics.SencondOrderUpdate(_target.position, _data2, Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private int _pierceStrenght = 0;
    private int _pierceCount = 0;
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speed;
    }

    void OnTriggerEnter(Collider other)
    {
        TerrainCell cell = other.GetComponent<TerrainCell>();
        if(cell)
        {
            cell.OnHit();
            _pierceCount++;
            if(_pierceCount >= _pierceStrenght)
                Destroy(gameObject);
        }
    }
}

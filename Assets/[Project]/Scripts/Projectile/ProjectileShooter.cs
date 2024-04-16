using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _aimTransform;
    [Space]
    [SerializeField] private float _attackSpeed = 2;
    [SerializeField] private int _multiShot;
    private bool _isShooting = false;
    private float _shootTimer = 0;

    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (!_isShooting)
            return;

        _shootTimer += Time.deltaTime * _attackSpeed;
        if (_shootTimer > 1)
        {
            GameObject newProj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            newProj.transform.rotation = _aimTransform.rotation;
            _shootTimer = 0f;
        }
    }

    private void OnShoot(InputValue value)
    {
        print("Shoot : " + value.Get<float>()); 
        if(value.Get<float>() > .5f)
            _isShooting = true;
        else
            _isShooting = false;
    }
}

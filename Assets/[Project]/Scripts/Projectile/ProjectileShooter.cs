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
    private InputAction _shootAction;
    private bool _canShoot = false;
    private float _shootTimer = 0f;

    void Start()
    {
        _shootAction = GetComponent<PlayerInput>().actions.FindAction("Shoot");
    }

    void Update()
    {
        _shootTimer += Time.deltaTime * _attackSpeed;
        _canShoot = _shootTimer > 1;

        if (_shootAction.ReadValue<float>() > .5f && _canShoot)
        {
            _shootTimer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject newProj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        newProj.transform.rotation = _aimTransform.rotation;
    }

    private void OnShoot(InputValue value)
    {


    }
}

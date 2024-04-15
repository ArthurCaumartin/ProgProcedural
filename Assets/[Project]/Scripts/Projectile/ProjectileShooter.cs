using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _aimTransform;

    private void Shoot()
    {
        GameObject newProj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        newProj.transform.rotation = _aimTransform.rotation;
    }

    private void OnShoot(InputValue value)
    {
        if (value.Get<float>() > .5f)
            Shoot();
    }
}

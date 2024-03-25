using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private float _speed;

    void OnValidate()
    {
        if(!_playerTransform)
            return;
        transform.position = _playerTransform.position + _offSet;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _playerTransform.position + _offSet, Time.deltaTime * _speed);
    }
}

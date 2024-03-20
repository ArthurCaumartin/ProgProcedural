using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _offSet;

    void OnValidate()
    {
        if(!_playerTransform)
            return;
        transform.position = _playerTransform.position + _offSet;
    }

    void Update()
    {
        transform.position = _playerTransform.position + _offSet;
    }
}

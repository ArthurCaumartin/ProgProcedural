using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Reseter : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Volume _resetVolume;
    [Space]
    [SerializeField] private bool _isResetHold = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _time;

    private CharacterMovement _characterMovement;
    private Vector3 _resetPoint;

    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _resetPoint = transform.position;
    }

    void Update()
    {
        if (_isResetHold)
            _time += Time.deltaTime * _speed;
        else
            _time = 0;

        _image.fillAmount = _time;
        _resetVolume.weight = _time;

        if (_time >= 1)
            ResetSequence();
    }

    private void ResetSequence()
    {
        enabled = false;

        Vector3 animationStartPoint = transform.position;
        _characterMovement.enabled = false;

        DOTween.To((time) =>
        {
            transform.position = Vector3.Lerp(animationStartPoint, _resetPoint, time);
        }, 0, 1, 1)
        .OnComplete(() =>
        {
            _characterMovement.enabled = true;
            enabled = true;
            _time = 0;
        });
    }

    private void OnReset(InputValue value)
    {
        _isResetHold = value.Get<float>() > .5f;
    }
}

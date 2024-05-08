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
    [SerializeField] private MineGenerator _mineGenerator;
    [SerializeField] private bool _isResetHold = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _time;

    private CharacterMovement _characterMovement;
    private ProjectileShooter _projectileShooter;
    private Vector3 _resetPoint;

    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _projectileShooter = GetComponent<ProjectileShooter>();
        _resetPoint = transform.position;
    }

    void Update()
    {
        if (_isResetHold)
            _time += Time.deltaTime * _speed;
        else
        {
            if(_time > 0)
                _time -= Time.deltaTime * _speed;
        }

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
        _projectileShooter.enabled = false;

        DOTween.To((time) =>
        {
            transform.position = Vector3.Lerp(animationStartPoint, _resetPoint, time);
        }, 0, 1, 1)
        .OnComplete(() =>
        {
            StartCoroutine(_mineGenerator.SpawnNewMine(() =>
            {
                DOTween.To((time) =>
                {
                    _time = time;
                    _image.fillAmount = _time;
                    _resetVolume.weight = _time;
                }, 1, 0, 2)
                .OnComplete(() =>
                {
                    _characterMovement.enabled = true;
                    _projectileShooter.enabled = true;
                    enabled = true;
                });
            }));
        });
    }

    private void OnReset(InputValue value)
    {
        _isResetHold = value.Get<float>() > .5f;
    }
}

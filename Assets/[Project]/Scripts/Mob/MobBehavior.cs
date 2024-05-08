using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    [SerializeField] private float _rotateTime = .05f;
    [SerializeField] private float _atkDelay = 3;
    [Space]
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private MobAttack _attackPrefabs;
    private MobAttack _currentAttack;
    private Transform _target;
    private Vector3 _startScale;
    private Vector3 _rotateVel = Vector3.zero;
    private float _attackTimer;

    void Start()
    {
        _startScale = transform.localScale;
    }

    void Update()
    {
        if(!_target)
            return;

        Vector3 forwardTarget = (_target.position - transform.position).normalized;
        // transform.forward = Vector3.Lerp(transform.forward, forwardTarget, Time.deltaTime * .5f);
        transform.forward = Vector3.SmoothDamp(transform.forward, forwardTarget, ref _rotateVel, _rotateTime, 1000);


        float dotToPlayer = Vector3.Dot(forwardTarget, transform.forward);
        _attackTimer += Time.deltaTime;

        if(dotToPlayer > .80f && _attackTimer > _atkDelay && !_currentAttack)
            Attack();
        // print(dotToPlayer);
    }

    private void Attack()
    {
        _attackTimer = 0;
        _currentAttack = Instantiate(_attackPrefabs, transform);
        _currentAttack.Initialize(_damage, _attackSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        print("Trigger : " + other.name);
        if(other.tag != "Player" || _target)
            return;
        
        GetComponent<SphereCollider>().radius = 1;
        _target = other.transform;

        transform.DOScale(transform.localScale * 1.5f, .15f)
        .OnComplete(() => transform.DOScale(_startScale, .15f));
    }
}

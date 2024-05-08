using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBehavior : MonoBehaviour
{
    private Transform _target;

    void Update()
    {
        if(!_target)
            return;
        Vector3 forwardTarget = (_target.position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, forwardTarget, Time.deltaTime * .5f);
    }

    void OnTriggerEnter(Collider other)
    {
        print("Trigger : " + other.name);
        if(other.tag != "Player")
            return;
        
        GetComponent<SphereCollider>().radius = 1;
        _target = other.transform;
    }
}

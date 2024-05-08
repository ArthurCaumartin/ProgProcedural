using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    [SerializeField] private Transform _previewCube;
    private float _time;
    private PlayerHealth _playerHealth;
    private float _damage;
    private float _speed;

    public void Initialize(float damage, float speed)
    {
        _damage = damage;
        _speed = speed;
    }

    void Update()
    {
        _time += Time.deltaTime * _speed;
        _previewCube.localScale = Vector3.Lerp(Vector3.one, new Vector3(2, 1, 10), _time);
        _previewCube.localPosition = new Vector3(0, 0, _previewCube.localScale.z / 2);

        if(_time > 1)
        {
            if(_playerHealth)
                _playerHealth.health -= (int)_damage;
            
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health)
            _playerHealth = health;
    }

    void OnTriggerExit(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health)
            _playerHealth = null;
    }
}

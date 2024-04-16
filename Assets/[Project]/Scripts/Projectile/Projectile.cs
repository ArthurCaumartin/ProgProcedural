using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int _currentPierceCount = 0;
    private int _pierceStrenght = 0;
    private int _damage;
    private float _speedMultiplier;

    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 10f);
    }

    public void Initialize(int damage, float speedMultiplier, int pierceCount)
    {
        _damage = damage;
        _speedMultiplier = speedMultiplier;
        _pierceStrenght = pierceCount;
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speedMultiplier;
    }

    void OnTriggerEnter(Collider other)
    {
        TerrainCell cell = other.GetComponent<TerrainCell>();
        if(cell)
        {
            cell.OnHit(_damage, this);
            _currentPierceCount++;
            if(_currentPierceCount >= _pierceStrenght)
                Destroy(gameObject);
        }
    }
}

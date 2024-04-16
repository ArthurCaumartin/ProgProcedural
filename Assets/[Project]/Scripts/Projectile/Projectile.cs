using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody _rigidbody;
    private int _currentPierceCount = 0;
    private int _pierceStrenght = 0;
    private int _damage = 1;
    private float _speed = 1;
    private bool _isAoe = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    public void Initialize(int damage, float speedMultiplier, int pierceCount, bool isAoe)
    {
        _damage = damage;
        _speed = speedMultiplier;
        _pierceStrenght = pierceCount;
        _isAoe = isAoe;
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speed;
    }

    void OnTriggerEnter(Collider other)
    {
        TerrainCell cell = other.GetComponent<TerrainCell>();
        if (!cell)
            return;

        if (!_isAoe)
        {
            cell.OnHit(_damage, this);
            _currentPierceCount++;
            if (_currentPierceCount >= _pierceStrenght)
                Destroy(gameObject);
            return;
        }
    
        RaycastHit hit = Physics.SphereCast(cell.transform.position
                                        , 1f
                                        , Vector3.zero
                                        , )



    }
}

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
    private Transform _player;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage, float speedMultiplier, int pierceCount, bool isAoe, Transform player)
    {
        _damage = damage;
        _speed = speedMultiplier;
        _pierceStrenght = pierceCount;
        _isAoe = isAoe;
        _player = player;
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speed;

        if(Vector3.Distance(_player.position, transform.position) > 5)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        TerrainCell hitCell = other.gameObject.GetComponent<TerrainCell>();
        if (!hitCell)
            return;

        if (!_isAoe)
        {
            hitCell.OnHit(_damage, this);
            ReducePierceCount();
        }

        if (_isAoe)
        {
            Collider[] colliderOverlap = Physics.OverlapSphere(hitCell.transform.position, 1.5f);

            for (int i = 0; i < colliderOverlap.Length; i++)
            {
                TerrainCell overlapCell = colliderOverlap[i].gameObject.GetComponent<TerrainCell>();
                print(colliderOverlap[i].gameObject.name);
                overlapCell?.OnHit(_damage, this);
                if(overlapCell)
                    Debug.DrawLine(hitCell.transform.position, overlapCell.transform.position, Color.green, 1.5f);
            }
            ReducePierceCount();
        }
    }

    private void ReducePierceCount()
    {
        _currentPierceCount++;
        if (_currentPierceCount >= _pierceStrenght)
            Destroy(gameObject);
    }
}

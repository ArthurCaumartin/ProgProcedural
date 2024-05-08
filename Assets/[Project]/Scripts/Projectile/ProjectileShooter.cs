using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _aimTransform;
    [SerializeField] private Transform _minAim;
    [SerializeField] private Transform _maxAim;

    [Space]
    [Header("STATS :")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _projectilSpeed = 1;
    [SerializeField] private int _peirceCount = 0;
    [SerializeField] private float _attackSpeed = 2;
    [SerializeField] private int _multiShot;
    [SerializeField] private bool _isAoeEnable = false;
    

    private List<GameObject> _shootPointList = new List<GameObject>();
    private InputAction _shootAction;
    private bool _canShoot = false;
    private float _shootTimer = 0f;
    private int _lastFrameMultiShot = 0;
    private MouseAim _mouseAim;
    private Vector3 startMaxAimAngle;
    private Vector3 startMinAimAngle;

    void Start()
    {
        _shootAction = GetComponent<PlayerInput>().actions.FindAction("Shoot");
        _mouseAim = GetComponentInChildren<MouseAim>();

        startMaxAimAngle = _maxAim.transform.localEulerAngles;
        startMinAimAngle = _minAim.transform.localEulerAngles;
    }

    void Update()
    {
        if (Application.isFocused)
            Shoot();

        if (_lastFrameMultiShot != _multiShot)
            CreateNewShootPoint();
        _lastFrameMultiShot = _multiShot;
        ComputeNewShootPointOrientation();
    }

    private void CreateNewShootPoint()
    {
        foreach (var item in _shootPointList)
        {
            Destroy(item);
        }
        _shootPointList.Clear();

        for (int i = 0; i < _multiShot; i++)
        {
            GameObject newPoint = Instantiate(new GameObject(), _aimTransform);
            newPoint.name = "ShootPoint_" + i;
            _shootPointList.Add(newPoint);
        }
    }

    private void ComputeNewShootPointOrientation()
    {
        float distance = Vector3.Distance(transform.position, _mouseAim.GetMousePos());
        float angleOffset = Mathf.Lerp(40, -25, Mathf.InverseLerp(0, 4, distance));

        _minAim.transform.localEulerAngles = new Vector3(startMinAimAngle.x
                                                , startMinAimAngle.y + angleOffset
                                                , startMinAimAngle.z);

        _maxAim.transform.localEulerAngles = new Vector3(startMaxAimAngle.x
                                                , startMaxAimAngle.y - angleOffset
                                                , startMaxAimAngle.z);

        if(_multiShot == 1)
        {
            _shootPointList[0].transform.rotation = 
            Quaternion.Slerp(_minAim.transform.rotation, _maxAim.transform.rotation, .5f);
            return;
        }

        for (int i = 0; i < _multiShot; i++)
        {
            float ratio = Mathf.InverseLerp(0, _multiShot - 1, i);
            _shootPointList[i].transform.rotation =
            Quaternion.Slerp(_minAim.transform.rotation, _maxAim.transform.rotation, ratio);
        }
    }

    private void Shoot()
    {
        _shootTimer += Time.deltaTime * _attackSpeed;
        _canShoot = _shootTimer > 1;

        if (_shootAction.ReadValue<float>() > .5f && _canShoot)
        {
            _shootTimer = 0;
            CreateProjectile();
        }

        void CreateProjectile()
        {
            foreach (var item in _shootPointList)
            {
                GameObject newProj = Instantiate(_projectilePrefab, item.transform.position, Quaternion.identity);
                newProj.transform.rotation = item.transform.rotation;
                newProj.GetComponent<Projectile>().Initialize(_damage, _projectilSpeed, _peirceCount, _isAoeEnable);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_shootPointList.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach (var item in _shootPointList)
                Gizmos.DrawRay(item.transform.position, item.transform.forward);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(_minAim.transform.position, _minAim.transform.forward);
            Gizmos.DrawRay(_maxAim.transform.position, _maxAim.transform.forward);
        }
    }
    public void UnlockShot()
    {
        _multiShot ++;
    } 
    public void BuffDamage()
    {
        _damage ++;
    }
    public void ProjectileSpeed()
    {
        _projectilSpeed ++;
    }
    public void BuffPierce()
    {
        _peirceCount ++;
    }
}

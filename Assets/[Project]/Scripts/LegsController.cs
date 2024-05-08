using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class LegData
{
    [SerializeField] public Transform raycastPoint;
    [SerializeField] public Transform groundPoint;
    [SerializeField] public Transform rigTarget;
    [Space]
    [SerializeField] public bool isAnimated = false;
    [SerializeField] private float animationSpeed = .2f;
    [SerializeField] private AnimationCurve horizontalCurve;
    [SerializeField] private AnimationCurve verticalCurve;
    private Vector3 startPosition;
    private float animationTime;

    public void StartAnimation()
    {
        isAnimated = true;
        startPosition = rigTarget.position;
        // Debug.Log("Start animation");
    }

    public void UpdateAnimation()
    {
        if (!isAnimated)
            return;

        animationTime += Time.deltaTime * animationSpeed;
        rigTarget.position =
        Vector3.Lerp(startPosition, groundPoint.position, horizontalCurve.Evaluate(animationTime));

        rigTarget.position =
        new Vector3(rigTarget.position.x, groundPoint.position.y + verticalCurve.Evaluate(animationTime), rigTarget.position.z);

        if (animationTime >= 1)
        {
            animationTime = 0;
            isAnimated = false;
        }
    }
}

public class LegsController : MonoBehaviour
{
    [SerializeField] private Transform _raycastPointsContainer;
    [SerializeField] private float _raycastPointsOffsetDistance = .3f;
    [SerializeField] private float _raycastPointsOffsetSpeed = 2f;
    [SerializeField] private LayerMask _groundLayer;
    [Space]
    [SerializeField] private float _animationDistanceTrigger = .3f;
    [Space]
    [SerializeField] private float _resetLegThresold = 1;
    [SerializeField] private float _resetLegDistanceThresold = .2f;
    [Space]
    [SerializeField] private float _averageHeightOffSet = .1f;
    [SerializeField] private List<LegData> _legList = new List<LegData>();
    private float _averageHeight;
    private Vector3 _inputAxis;
    private float _resetLegTimer;
    private bool _canReset = true;

    void Update()
    {
        if (_canReset)
            ResetLegPosition();

        ComputeGroundPoint();
        ComputeRaycastContainerOffset();
        StartLegsAnimation();
        UpdateYPosition();

        foreach (var item in _legList)
            item.UpdateAnimation();
    }

    private void UpdateYPosition()
    {
        transform.position = new Vector3(transform.position.x, GetAverageHeight(), transform.position.z);   
    }

    private void ResetLegPosition()
    {
        //TODO ajouter un delais infime pour les start animation
        if (_inputAxis == Vector3.zero)
            _resetLegTimer += Time.deltaTime;
        else
            _resetLegTimer = 0;

        if (_resetLegTimer > _resetLegThresold)
        {
            foreach (var item in _legList)
            {
                if (Vector3.Distance(item.rigTarget.position, item.groundPoint.position) > _resetLegDistanceThresold)
                    item.StartAnimation();
            }
            _canReset = false;
        }
    }

    private void ComputeRaycastContainerOffset()
    {
        _raycastPointsContainer.transform.localPosition
         = Vector3.Lerp(_raycastPointsContainer.transform.localPosition, _inputAxis * _raycastPointsOffsetDistance
                        , Time.deltaTime * _raycastPointsOffsetSpeed);
    }

    private void StartLegsAnimation()
    {
        int animatedLegs = 0;
        foreach (var item in _legList)
            if (item.isAnimated)
                animatedLegs++;

        if (animatedLegs > 3)
            return;


        float maxDistance = -Mathf.Infinity;
        LegData farestLeg = null;

        foreach (var item in _legList)
        {
            float currentDistance = Vector3.Distance(item.rigTarget.position, item.groundPoint.position);
            if(currentDistance > maxDistance && currentDistance > _animationDistanceTrigger)
            {
                maxDistance = currentDistance;
                farestLeg = item;
            }
        }
        
        farestLeg?.StartAnimation();            
    }

    private void ComputeGroundPoint()
    {
        foreach (var item in _legList)
        {
            RaycastHit hit;
            Physics.Raycast(item.raycastPoint.position, Vector3.down, out hit, Mathf.Infinity, _groundLayer);
            if (hit.collider)
            {
                item.groundPoint.position = hit.point;
            }
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _inputAxis = inputValue.Get<Vector2>();
        _inputAxis.z = _inputAxis.y;
        _inputAxis.y = 0;

        _canReset = true;
    }

    public float GetAverageHeight()
    {
        _averageHeight = 0;
        foreach (var item in _legList)
            _averageHeight += item.groundPoint.position.y;

        return _averageHeight = (_averageHeight / _legList.Count) + _averageHeightOffSet;
    }
}

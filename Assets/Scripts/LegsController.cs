using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class LegData
{
    [SerializeField] public Transform raycastPoint;
    [SerializeField] public Transform groundPoint;
    [SerializeField] public Transform rigTarget;
    [Space]
    [SerializeField] public bool isAnimated = false;
    [SerializeField] private float animationSpeed = .2f;
    [SerializeField] private AnimationCurve curve;
    private Vector3 startPosition;
    private float animationTime;

    public void StartAnimation()
    {
        isAnimated = true;
        startPosition = rigTarget.position;
    }

    public void UpdateAnimation()
    {
        if (!isAnimated)
            return;

        animationTime += Time.deltaTime * animationSpeed;
        rigTarget.position =
        Vector3.Lerp(startPosition, groundPoint.position, curve.Evaluate(animationTime));

        if (animationTime >= 1)
        {
            animationTime = 0;
            isAnimated = false;
        }
    }
}

public class LegsController : MonoBehaviour
{
    [SerializeField] private Transform _raycastPointContainer;
    [SerializeField] private float _raycastPointsOffsetDistance = .3f;
    [SerializeField] private float _raycastPointsOffsetSpeed = 2f;
    [Space]
    [SerializeField] private float _animationDistanceTrigger = .3f;
    [SerializeField] private List<LegData> legList = new List<LegData>();
    private Vector3 _inputAxis;

    void Update()
    {
        ComputeGroundPoint();
        ComputeRaycastContainerOffset();
        StartLegsAnimation();

        foreach (var item in legList)
            item.UpdateAnimation();
    }

    private void ComputeRaycastContainerOffset()
    {
        _raycastPointContainer.transform.localPosition
         = Vector3.Lerp(_raycastPointContainer.transform.localPosition, _inputAxis * _raycastPointsOffsetDistance
                        , Time.deltaTime * _raycastPointsOffsetSpeed);
    }

    private void StartLegsAnimation()
    {
        int animatedLegs = 0;
        foreach (var item in legList)
            if(item.isAnimated)
                animatedLegs++;
        
        if(animatedLegs > 3)
            return;

        foreach (var item in legList)
        {
            if (Vector3.Distance(item.rigTarget.position, item.groundPoint.position) > _animationDistanceTrigger)
            {
                item.StartAnimation();
                return;
            }
        }
    }

    private void ComputeGroundPoint()
    {
        foreach (var item in legList)
        {
            RaycastHit hit;
            Physics.Raycast(item.raycastPoint.position, Vector3.down, out hit);
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
    }

    public float GetAverageHeight()
    {
        float height = 0;
        foreach (var item in legList)
            height += item.groundPoint.position.y;
        
        return height /= legList.Count;
    }
}

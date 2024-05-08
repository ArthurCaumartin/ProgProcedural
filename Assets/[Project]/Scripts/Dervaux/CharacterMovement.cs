using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private float _speed = 5; //m/s

    [SerializeField, Range(0, 100)] private float _gravityFactor = 9.81f;

    Rigidbody _rigidBody;

    Vector2 _inputMovement = Vector2.zero;
    Vector2 _inputDash = Vector2.zero;

    Vector3 _horizontalVelocity = Vector3.zero;
    Vector3 _verticalVelocity = Vector3.zero;

    Vector3 _smoothDampVelocity = Vector3.zero;
    float _smoothTime = 0.1f;
    float _maxSpeed = 50;
    float dashSpeed = 10f;
    float dashduration = 0.5f;
    private bool isDashing = false;
    private bool isDashingEnabled = false;

    private FloorSensor _floorSensor;

    public void Move(Vector2 inputMovement)
    {
        _inputMovement = inputMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _floorSensor = GetComponent<FloorSensor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        Vector3 groundCorrection = Vector3.zero;

        if (_floorSensor.IsFloorDetected() && _floorSensor.GetFloorDistance() < 0)
        {

            _verticalVelocity = Vector3.zero;

            if (_floorSensor.GetFloorDistance() < -0.01f)
            {
                groundCorrection = -_floorSensor.GetFloorDistance() * transform.up;
            }
        }
        else
        {
            _verticalVelocity += _gravityFactor * Time.fixedDeltaTime * Vector3.down;
        }

        Vector3 targetVelocity = new Vector3(_inputMovement.x, 0, _inputMovement.y) * _speed;

        _horizontalVelocity = Vector3.SmoothDamp(_horizontalVelocity, targetVelocity, ref _smoothDampVelocity, _smoothTime, _maxSpeed);

        _rigidBody.velocity = _horizontalVelocity + _verticalVelocity + groundCorrection * Time.fixedDeltaTime;
    }
    public void AddSpeed()
    {
        _speed += 0.2f;
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && isDashingEnabled == true)
        {
            Debug.Log("Dash");
            StartCoroutine(Dash());
        }
    }
     IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashduration)
        {
            _rigidBody.velocity = new Vector3(_inputMovement.x, 0, _inputMovement.y) * dashSpeed;
            yield return null;
        }

        isDashing = false;
        _rigidBody.velocity = Vector3.zero;
    }
    public void DashUnlock()
    {
        isDashingEnabled = true;
    }
}

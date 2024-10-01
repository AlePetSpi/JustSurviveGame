using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private int _vehicleId;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private int _steering;
    [SerializeField] private int _power;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] public AxleInfo[] axleInfos;
    private Vector2 _direction;
    private Renderer[] _cartRenderers;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    public AxleInfo[] AxleInfos => axleInfos;
    public int VehicleId { get => _vehicleId; set => _vehicleId = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Shield { get => _shield; set => _shield = value; }
    public int Steering { get => _steering; set => _steering = value; }
    public int Power { get => _power; set => _power = value; }
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    public void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _cartRenderers = GetComponentsInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnReset()
    {
        _rb.velocity = Vector3.zero;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    private void OnMove(InputValue inputValue)
    {
        _direction = inputValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        float acceleration = _direction.y * _power;
        float steering = _direction.x * _steering;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.Motor)
            {
                axleInfo.LeftWheel.motorTorque = acceleration;
                axleInfo.RightWheel.motorTorque = acceleration;
            }

            if (axleInfo.Steering)
            {
                axleInfo.LeftWheel.steerAngle = steering;
                axleInfo.RightWheel.steerAngle = steering;
            }
        }
    }

    public float GetSpeed()
    {
        return _rb.velocity.magnitude;
    }

    public float GetMaxSlip()
    {
        float slip1 = GetSingleTireSlip(axleInfos[1].LeftWheel);
        float slip2 = GetSingleTireSlip(axleInfos[1].RightWheel);
        return Mathf.Max(slip1, slip2);
    }

    private float GetSingleTireSlip(WheelCollider wheelCollider)
    {
        bool sliding = false;
        if (!wheelCollider.isGrounded)
        {
            return 0;
        }

        wheelCollider.GetGroundHit(out WheelHit wheelHit);

        float sidewaysSlip = Math.Abs(wheelHit.sidewaysSlip);

        if (sidewaysSlip > wheelCollider.sidewaysFriction.extremumSlip - 0.1f)
        {
            sliding = true;
        }

        float forwardSlip = wheelHit.forwardSlip;
        if (forwardSlip > Math.Abs(wheelCollider.forwardFriction.asymptoteSlip))
        {
            sliding = true;
        }

        if (sliding)
        {
            return wheelCollider.attachedRigidbody.velocity.magnitude;
        }
        else
        {
            return 0;
        }
    }

}
[Serializable]
public class AxleInfo
{
    [SerializeField] private WheelCollider leftWheel;
    [SerializeField] private WheelCollider rightWheel;
    [SerializeField] private bool motor;
    [SerializeField] private bool steering;

    public WheelCollider LeftWheel => leftWheel;
    public WheelCollider RightWheel => rightWheel;
    public bool Motor => motor;
    public bool Steering => steering;
}
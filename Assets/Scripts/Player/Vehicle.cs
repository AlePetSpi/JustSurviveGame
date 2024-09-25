using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private int _vehicleId;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private int _steering;
    [SerializeField] private int _power;
    [SerializeField] private Rigidbody _rigidbody;

    public int VehicleId { get => _vehicleId; set => _vehicleId = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Shield { get => _shield; set => _shield = value; }
    public int Steering { get => _steering; set => _steering = value; }
    public int Power { get => _power; set => _power = value; }
    public Rigidbody Rb { get => _rigidbody; set => _rigidbody = value; }

}

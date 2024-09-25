using Assets.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _size = 10;
    [SerializeField] private float _speed = 10;
    [SerializeField] private SceneUIManager sceneUIManager;
    [SerializeField] private Vehicle[] vehicles;

    private int _maxHealth;
    private int _maxShield;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        if (vehicles == null) return;
        Debug.Log("Player awake is called");
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        //FindObjectOfType<GoalManager>().FinishedLap += OnFinishedLap;
        PersistentDataManager.DataChangedEvent += (_, _) => UpdatePlayerProperties();
        GameObject gameObjectChild = Instantiate(vehicles[PersistentDataManager.VehicleId].gameObject, gameObject.transform);
        _rigidbody = gameObjectChild.GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
        _maxHealth = vehicles[PersistentDataManager.VehicleId].MaxHealth;
        _maxShield = vehicles[PersistentDataManager.VehicleId].Shield;
        Debug.Log($"Vehicle NUmber {PersistentDataManager.VehicleId} / _maxHealth {_maxHealth} / ");
    }

    private void UpdatePlayerProperties()
    {
        _maxHealth = PersistentDataManager.Health;
        _maxShield = PersistentDataManager.Shield;
    }

    private void Update()
    {
        var dir = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);

        _rigidbody.velocity = dir;
    }

    private void OnReset()
    {
        PersistentDataManager.Health = gameObject.GetComponent<Vehicle>().MaxHealth;
        _rigidbody.velocity = Vector3.zero;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    public void Hit(int damage)
    {
        Debug.Log($"Player health is currently: {_maxHealth}");
        Debug.Log($"Hit damage: {damage}");
        _maxHealth -= damage;
        PersistentDataManager.Health = _maxHealth;
        if (_maxHealth <= 0)
        {
            Debug.Log("Player dead");
            Destroy(gameObject);
            sceneUIManager.EndScreen(EndScreenStatus.DEAD_STATUS);
        }
    }
}
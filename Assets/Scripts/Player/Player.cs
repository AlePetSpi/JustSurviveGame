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
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        //FindObjectOfType<GoalManager>().FinishedLap += OnFinishedLap;
        Debug.Log("Vehicle ID on Awake: " + PersistentDataManager.VehicleId);
        Debug.Log("Health on Awake: " + PersistentDataManager.Health);
        Debug.Log("Shield on Awake: " + PersistentDataManager.Shield);
        PersistentDataManager.DataChangedEvent += (_, _) => UpdatePlayerProperties();
        UpdatePlayerProperties();
    }

    private void UpdatePlayerProperties()
    {
        GameObject gameObjectChild = Instantiate(vehicles[PersistentDataManager.VehicleId].gameObject, gameObject.transform);
        _rigidbody = gameObjectChild.GetComponent<Rigidbody>();
        _maxHealth = (int)PersistentDataManager.Health;
        _maxShield = (int)PersistentDataManager.Shield;
        Debug.Log($"Vehicle NUmber {PersistentDataManager.VehicleId} / _maxHealth {_maxHealth} / ");

    }

    private void FixedUpdate()
    {
        var dir = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);

        //_rigidbody.velocity = dir;
    }

    private void OnReset()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    public void Hit(int damage)
    {
        Debug.Log($"Player health is currently: {_maxHealth}");
        if (_maxHealth <= 0)
        {
            Debug.Log("Player dead");
            Destroy(gameObject);
            sceneUIManager.EndScreen(EndScreenStatus.DEAD_STATUS);
        }
        else
        {
            Debug.Log($"Hit damage: {damage}");
            _maxHealth -= damage;
        }
    }
}
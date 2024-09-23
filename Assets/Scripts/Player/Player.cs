using Assets.Scripts;
using UnityEngine;

public class Player : MonoBehaviour, IExplode
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _size = 10;
    [SerializeField] private float _speed = 10;
    [SerializeField] private SceneUIManager sceneUIManager;
    public Rigidbody Rb => _rb;

    private int _maxHealth;
    private int _maxShield;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        //FindObjectOfType<GoalManager>().FinishedLap += OnFinishedLap;
        _rigidbody = GetComponent<Rigidbody>();

        PersistentDataManager.DataChangedEvent += (_, _) => UpdatePlayerProperties();
        UpdatePlayerProperties();

    }

    private void UpdatePlayerProperties()
    {
        _maxHealth = (int)PersistentDataManager.Health;
        _maxShield = (int)PersistentDataManager.Shield;
    }

    private void FixedUpdate()
    {
        var dir = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);

        _rb.velocity = dir;

    }

    private void OnReset()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    public void Hit(int damage)
    {

        Debug.Log($"Target health is currently: {_maxHealth}");
        if (_maxHealth <= 0)
        {
            Debug.Log("Target dead");
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
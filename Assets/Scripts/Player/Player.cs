using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _size = 10;
    [SerializeField] private float _speed = 10;
    [SerializeField] private SceneUIManager sceneUIManager;
    [SerializeField] private Vehicle[] vehicles;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ShieldBar shieldBar;
    [SerializeField] private Shield shieldPrefab;

    private int _currentHealth;
    private int _currentShield;
    private bool _shieldActive = false;
    private bool _shieldOnCooldown = false;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _rb;

    private Shield _activeShield;
    private GameObject _vehicleInstance;

    public ShieldBar ShieldBar { get => shieldBar; set => shieldBar = value; }

    public event EventHandler PauseMenuButtonPressed;
    protected virtual void OnPauseMenuButtonPressed()
    {
        PauseMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        if (vehicles == null) return;

        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        //_vehicleInstance = Instantiate(vehicles[PersistentDataManager.VehicleId].gameObject, gameObject.transform);
        _vehicleInstance = Instantiate(vehicles[0].gameObject, gameObject.transform);
        _rb = _vehicleInstance.GetComponent<Rigidbody>();
        _rb.useGravity = true;

        /*PersistentDataManager.MaxHealth = vehicles[PersistentDataManager.VehicleId].MaxHealth;
        PersistentDataManager.MaxShield = vehicles[PersistentDataManager.VehicleId].Shield;*/

        _currentHealth = PersistentDataManager.MaxHealth;
        healthBar.SetMaxHealth(PersistentDataManager.MaxHealth);

        _currentShield = PersistentDataManager.MaxShield;
        ShieldBar.SetMaxShield(PersistentDataManager.MaxShield);
        ShieldBar.SetShield(_currentShield);
    }

    private void OnPause()
    {
        PauseMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnShield()
    {
        if (_shieldActive || _shieldOnCooldown)
        {
            Debug.Log("Shield is either already active or on cooldown.");
            return;
        }

        Debug.Log("Shield is activated");
        _shieldActive = true;

        _activeShield = Instantiate(shieldPrefab, _vehicleInstance.transform);

        StartCoroutine(DrainShield());
    }

    private IEnumerator DrainShield()
    {
        int activeTimeSeconds = PersistentDataManager.MaxShield;
        int shieldDrainRate = _currentShield / activeTimeSeconds;
        Debug.Log($"Starting shield drain. Initial shield: {_currentShield}, Drain rate: {shieldDrainRate} per second.");

        while (_currentShield > 0 && activeTimeSeconds > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentShield -= shieldDrainRate;
            _currentShield = Mathf.Max(0, _currentShield);
            ShieldBar.SetShield(_currentShield);

            Debug.Log($"Shield value after {activeTimeSeconds} second(s): {_currentShield}");
            activeTimeSeconds--;
        }

        if (_currentShield <= 0)
        {
            Debug.Log("Shield fully drained.");
            Destroy(_activeShield.gameObject);
            Debug.Log("Shield destroyed");
        }

        yield return StartCoroutine(ShieldCooldown());
    }

    private IEnumerator ShieldCooldown()
    {
        _shieldActive = false;
        _shieldOnCooldown = true;
        Debug.Log("Starting shield recharge (cooldown).");

        int coolDownTimeSeconds = PersistentDataManager.MaxShield;
        int shieldRechargeRate = PersistentDataManager.MaxShield / coolDownTimeSeconds;
        Debug.Log($"Recharge rate: {shieldRechargeRate} per second, Max Shield: {PersistentDataManager.MaxShield}");

        _currentShield = 0;

        int secondsPassed = 0;
        while (_currentShield < PersistentDataManager.MaxShield)
        {
            yield return new WaitForSeconds(1f);
            _currentShield += shieldRechargeRate;
            _currentShield = Mathf.Min(_currentShield, PersistentDataManager.MaxShield);
            ShieldBar.SetShield(_currentShield);

            secondsPassed++;
            Debug.Log($"Recharge time: {secondsPassed} seconds, Current shield value: {_currentShield}");
        }

        Debug.Log("Shield fully recharged.");
        _shieldOnCooldown = false;
    }

    public void Hit(int damage)
    {
        if (_activeShield)
        {
            return;
        }
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            Debug.Log("Player dead");
            Destroy(gameObject);
            sceneUIManager.EndScreen(EndScreenStatus.DEAD_STATUS);
        }
    }
}

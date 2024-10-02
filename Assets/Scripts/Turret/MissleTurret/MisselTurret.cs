using System.Collections;
using UnityEngine;

public class MisselTurret : MonoBehaviour
{
    [SerializeField] private MountPoint[] mountPoints;
    [SerializeField] private Transform[] missilePoints;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int seconds;

    private bool _isActivated = false;
    private Vehicle _vehicle;
    private int _startMissileNr = 0;
    private Coroutine _missileRoutine;
    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_source)
        {
            if (Time.timeScale == 0)
            {
                _source.Pause();
            }
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _vehicle = collider.GetComponent<Vehicle>();
            _isActivated = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _vehicle = null;
            _isActivated = false;

            if (_missileRoutine != null)
            {
                StopCoroutine(_missileRoutine);
                _missileRoutine = null;
            }
        }
    }

    private void FixedUpdate()
    {

        //Debug.Log($"Health {PersistentDataManager.Health}");
        if (!_vehicle || missilePoints.Length == 0)
        {
            //Debug.Log($"No Update for Missile Turret");
            return;
        }

        var aimed = true;
        foreach (var mountPoint in mountPoints)
        {
            if (!mountPoint.Aim(_vehicle.transform))
            {
                aimed = false;
            }
        }

        if (aimed && _isActivated && _startMissileNr < missilePoints.Length && _missileRoutine == null)
        {
            Debug.Log($"Health {PersistentDataManager.MaxHealth}");
            _missileRoutine = StartCoroutine(StartMissileRoutine());
        }
    }

    private IEnumerator StartMissileRoutine()
    {
        while (_isActivated && _startMissileNr < missilePoints.Length)
        {
            FireMissile();
            yield return new WaitForSeconds(seconds);
            if (!_isActivated || PersistentDataManager.MaxHealth <= 0)
            {
                Debug.Log($"Wait Seconds break");
                yield break;
            }

        }
        _missileRoutine = null;
    }

    private void FireMissile()
    {
        if (_startMissileNr >= missilePoints.Length) return;

        Transform missilePoint = missilePoints[_startMissileNr];
        var missile = Instantiate(missilePrefab, missilePoint.position, missilePoint.rotation);
        missile.GetComponent<Missile>().Vehicle = _vehicle;
        Debug.Log($"Missile {_startMissileNr} launch!");
        _startMissileNr++;
        missilePoint.gameObject.SetActive(false);

    }
}

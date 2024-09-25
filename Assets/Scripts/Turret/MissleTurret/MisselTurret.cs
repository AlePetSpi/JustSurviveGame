using System.Collections;
using UnityEditor;
using UnityEngine;

public class MisselTurret : MonoBehaviour
{
    [SerializeField] private MountPoint[] mountPoints;
    [SerializeField] private Transform[] missilePoints;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int seconds;
    [SerializeField] private bool drawGizmos = false;

    private bool _isActivated = false;
    private Vehicle _vehicle;
    private int _startMissileNr = 0;
    private Coroutine _missileRoutine;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!drawGizmos || !_vehicle) return;

        var dashLineSize = 2f;

        foreach (var mountPoint in mountPoints)
        {
            var hardpoint = mountPoint.transform;
            var from = Quaternion.AngleAxis(-mountPoint.angleLimit / 2, hardpoint.up) * hardpoint.forward;
            var projection = Vector3.ProjectOnPlane(_vehicle.transform.position - hardpoint.position, hardpoint.up);

            Handles.color = Color.white;
            Handles.DrawDottedLine(_vehicle.transform.position, hardpoint.position + projection, dashLineSize);

            if (Vector3.Angle(hardpoint.forward, projection) > mountPoint.angleLimit / 2) return;

            Handles.color = Color.red;
            Handles.DrawLine(hardpoint.position, hardpoint.position + projection);

            Handles.color = Color.green;
            Handles.DrawWireArc(hardpoint.position, hardpoint.up, from, mountPoint.angleLimit, projection.magnitude);
            Handles.DrawSolidDisc(hardpoint.position + projection, hardpoint.up, .5f);
#endif
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

        //Debug.Log($"aimed {aimed} / _isActivated {_isActivated} / _startMissileNr {_startMissileNr} < missilePoints.Length {missilePoints.Length} / _missileRoutine {_missileRoutine}");
        if (aimed && _isActivated && _startMissileNr < missilePoints.Length && _missileRoutine == null)
        {
            Debug.Log($"Health {PersistentDataManager.Health}");
            _missileRoutine = StartCoroutine(StartMissileRoutine());
        }
    }

    private IEnumerator StartMissileRoutine()
    {
        while (_isActivated && _startMissileNr < missilePoints.Length)
        {
            FireMissile();
            yield return new WaitForSeconds(seconds);
            if (!_isActivated || PersistentDataManager.Health <= 0)
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
        Debug.Log($"Missile {_startMissileNr} abgefeuert!");
        _startMissileNr++;
        missilePoint.gameObject.SetActive(false);

    }
}

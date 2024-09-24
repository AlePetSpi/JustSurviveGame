using System.Collections;
using UnityEditor;
using UnityEngine;

public class MisselTurret : MonoBehaviour
{
    [SerializeField] private Missile[] missiles;
    [SerializeField] private MountPoint[] mountPoints;
    [SerializeField] private int seconds;

    private bool _isActivated = false;
    //private Transform _playerTransform;
    private Player _player;
    private int _startMissleNr = 0;

    private System.Diagnostics.Stopwatch _stopWatch = new System.Diagnostics.Stopwatch();

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_player) return;

        var dashLineSize = 2f;

        foreach (var mountPoint in mountPoints)
        {
            var hardpoint = mountPoint.transform;
            var from = Quaternion.AngleAxis(-mountPoint.angleLimit / 2, hardpoint.up) * hardpoint.forward;
            var projection = Vector3.ProjectOnPlane(_player.transform.position - hardpoint.position, hardpoint.up);

            // projection line
            Handles.color = Color.white;
            Handles.DrawDottedLine(_player.transform.position, hardpoint.position + projection, dashLineSize);

            // do not draw target indicator when out of angle
            if (Vector3.Angle(hardpoint.forward, projection) > mountPoint.angleLimit / 2) return;

            // target line
            Handles.color = Color.red;
            Handles.DrawLine(hardpoint.position, hardpoint.position + projection);

            // range line
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
            //_playerTransform = collider.transform;
            _player = collider.GetComponent<Player>();
            _isActivated = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //_playerTransform = null;
            _player = null;
            _isActivated = false;
        }
    }

    private void FixedUpdate()
    {
        // do nothing when no target
        if (!_player) return;

        // aim target
        var aimed = true;
        foreach (var mountPoint in mountPoints)
        {
            if (!mountPoint.Aim(_player.transform))
            {
                aimed = false;
            }
        }

        // shoot when aimed
        if (aimed && _isActivated)
        {
            StartCoroutine(StartMissle());
        }
    }

    IEnumerator StartMissle()
    {
        yield return new WaitForSeconds(seconds);
        missiles[_startMissleNr].Player = _player;
        _startMissleNr++;
    }
}

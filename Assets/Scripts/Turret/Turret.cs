using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Gun gun;
    [SerializeField] private MountPoint[] mountPoints;

    private bool _isActivated = false;
    private Transform _target;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_target) return;

        var dashLineSize = 2f;

        foreach (var mountPoint in mountPoints)
        {
            var hardpoint = mountPoint.transform;
            var from = Quaternion.AngleAxis(-mountPoint.angleLimit / 2, hardpoint.up) * hardpoint.forward;
            var projection = Vector3.ProjectOnPlane(_target.position - hardpoint.position, hardpoint.up);

            // projection line
            Handles.color = Color.white;
            Handles.DrawDottedLine(_target.position, hardpoint.position + projection, dashLineSize);

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
        if (collider.gameObject.CompareTag("Target"))
        {
            _target = collider.transform;
            _isActivated = true;
            Debug.Log("Trigger enter and _isActivated: " + _isActivated);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Target"))
        {
            _target = null;
            _isActivated = false;
            Debug.Log("Trigger exit and _isActivated: " + _isActivated);
        }
    }

    private void FixedUpdate()
    {
        // do nothing when no target
        if (!_target) return;

        // aim target
        var aimed = true;
        foreach (var mountPoint in mountPoints)
        {
            if (!mountPoint.Aim(_target.position))
            {
                Debug.Log("Aim false");
                aimed = false;
            }
        }

        // shoot when aimed
        if (aimed && _isActivated)
        {
            Debug.Log("Gun is fired");
            gun.Fire();
        }
    }
}

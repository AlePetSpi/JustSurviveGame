using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Range(0.01f, 2f)] private float positionSmoothTime = 0.1f;
    [SerializeField, Range(0.01f, 2f)] private float rotationSmoothTime = 0.1f;
    private Vehicle _vehicle;
    private Vector3 _currVerlocity;
    private Vector3 _offset = new Vector3(0, 5f, -3.5f);

    private void FixedUpdate()
    {
        if (_vehicle == null) return;
        Vector3 targetPosition = _vehicle.transform.position + _vehicle.transform.TransformDirection(_offset);
        Vector3 currPosition = transform.position;
        transform.position = Vector3.SmoothDamp(currPosition, targetPosition, ref _currVerlocity, positionSmoothTime);

        Quaternion targetRotation = Quaternion.LookRotation(_vehicle.transform.position - transform.position);
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSmoothTime);
    }

    private void Start()
    {
        _offset = transform.localPosition;
        transform.SetParent(null, true);
        _vehicle = FindObjectOfType<Vehicle>();
    }
}

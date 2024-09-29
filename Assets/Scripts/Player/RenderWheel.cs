using UnityEngine;

public class RenderWheel : MonoBehaviour
{
    [SerializeField] public WheelPosition wheelPosition;
    private WheelCollider _wheelCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vehicle cart = FindObjectOfType<Vehicle>();
        switch (wheelPosition)
        {
            case WheelPosition.FL:
                _wheelCollider = cart.axleInfos[0].LeftWheel;
                break;
            case WheelPosition.FR:
                _wheelCollider = cart.axleInfos[0].RightWheel;
                break;
            case WheelPosition.RL:
                _wheelCollider = cart.axleInfos[1].LeftWheel;
                break;
            case WheelPosition.RR:
                _wheelCollider = cart.axleInfos[1].RightWheel;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        transform.position = pos;
        transform.rotation = rot;

    }

    public enum WheelPosition
    {
        FL,
        FR,
        RL,
        RR
    }
}
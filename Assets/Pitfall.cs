using UnityEngine;

public class Pitfall : MonoBehaviour
{
    [SerializeField] private Player player;
    private void OnTriggerEnter(Collider collider)
    {
        if (player != null)
        {
            if (collider.transform.TryGetComponent<Vehicle>(out var vehicle))
            {
                Player player = vehicle.GetComponentInParent<Player>();

                if (player != null)
                {
                    player.InstantKill();
                }
            }
        }
    }
}

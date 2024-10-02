using UnityEngine;

public class ExplosioDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 50;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.TryGetComponent<Vehicle>(out var vehicle))
        {
            Player player = vehicle.GetComponentInParent<Player>();

            if (player != null)
            {
                player.Hit(damageAmount);
            }
        }
    }
}

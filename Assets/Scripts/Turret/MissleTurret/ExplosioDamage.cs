using UnityEngine;

public class ExplosioDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 50;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Object collision with missile explosion");
        if (collider.transform.TryGetComponent<Vehicle>(out var vehicle))
        {
            Debug.Log("Vehicle collision with missile explosion");
            Player player = vehicle.GetComponentInParent<Player>();

            if (player != null)
            {
                player.Hit(damageAmount);
            }
        }
    }
}

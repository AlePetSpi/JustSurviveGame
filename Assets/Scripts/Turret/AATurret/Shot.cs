using UnityEngine;

public class Shot : MonoBehaviour, Projectiles
{
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float rangeOfBullet = 100;
    [SerializeField] private int damage = 5;

    private Vector3 velocity;

    void Start()
    {
        var muzzleEffect = Instantiate(muzzlePrefab, transform.position, transform.rotation);
        Destroy(muzzleEffect, 1f);
        velocity = transform.forward * speed;
        transform.Rotate(90.0f, 0.0f, 0.0f);
    }

    void FixedUpdate()
    {
        var displacement = velocity * Time.deltaTime;
        rb.MovePosition(rb.position + displacement);
        if (Vector3.Distance(velocity, rb.position) > rangeOfBullet)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hitEffect = Instantiate(hitPrefab, collision.GetContact(0).point, Quaternion.identity);
        if (collision.transform.TryGetComponent<Vehicle>(out var vehicle))
        {
            var player = vehicle.GetComponentInParent<Player>();

            if (player != null)
            {
                player.Hit(damage);
            }
        }
        Destroy(hitEffect, 2f);
    }
}

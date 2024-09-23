using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private float speed;
    [SerializeField] private float rangeOfBullet = 100;
    [SerializeField] private int damage = 5;

    Rigidbody rb;
    Vector3 velocity;

    void Awake()
    {
        TryGetComponent(out rb);
    }

    void Start()
    {
        var muzzleEffect = Instantiate(muzzlePrefab, transform.position, transform.rotation);
        Destroy(muzzleEffect);
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
        var hitEffect = Instantiate(hitPrefab, collision.GetContact(0).point, Quaternion.identity);
        if (collision.transform.TryGetComponent<IExplode>(out var ex)) ex.Hit(damage);
        Destroy(hitEffect, 2f);
        Destroy(gameObject);
    }
}

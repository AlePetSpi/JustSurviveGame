using UnityEngine;

public class GatlingGun : MonoBehaviour
{
    [SerializeField] private Shot shotPrefab;

    // Gameobjects need to control rotation and aiming
    [SerializeField] private Transform goBaseRotation;
    [SerializeField] private Transform goGunBody;
    [SerializeField] private Transform goBarrel;

    // Distance the turret can aim and fire from
    [SerializeField] private float firingRange;

    // Particle system for the muzzel flash
    [SerializeField] private ParticleSystem muzzelFlash;
    // Gun barrel rotation
    [SerializeField] private float barrelRotationSpeed;
    [SerializeField] private float fireRate;

    private bool _firing;
    private float _fireTimer;

    // target the gun will aim at
    private Transform _player;

    private float _currentRotationSpeed;

    // Used to start and stop the turret firing
    private bool _canFire = false;


    private void Start()
    {
        // Set the firing range distance
        this.GetComponent<SphereCollider>().radius = firingRange;
    }

    private void FixedUpdate()
    {
        AimAndFire();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position to show the firing range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRange);
    }

    // Detect an Enemy, aim and fire
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _player = collider.transform;
            _canFire = true;
        }

    }
    // Stop firing
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = null;
            _canFire = false;
        }
    }

    private void AimAndFire()
    {
        // Gun barrel rotation
        goBarrel.transform.Rotate(0, 0, _currentRotationSpeed * Time.deltaTime);

        // if can fire turret activates
        if (_canFire && _player)
        {
            // start rotation
            _currentRotationSpeed = barrelRotationSpeed;

            // aim at enemy
            Vector3 baseTargetPostition = new Vector3(_player.position.x, this.transform.position.y, _player.position.z);
            Vector3 gunBodyTargetPostition = new Vector3(_player.position.x, _player.position.y, _player.position.z);

            goBaseRotation.transform.LookAt(baseTargetPostition);
            goGunBody.transform.LookAt(gunBodyTargetPostition);

            // start particle system 
            if (!muzzelFlash.isPlaying)
            {
                muzzelFlash.Play();
            }

            while (_fireTimer >= 1 / fireRate)
            {
                Instantiate(shotPrefab, goBarrel.position, goBarrel.rotation);
                _fireTimer -= 1 / fireRate;
            }

            _fireTimer += Time.deltaTime;
        }
        else
        {
            // slow down barrel rotation and stop
            _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, 0, 10 * Time.deltaTime);

            // stop the particle system
            if (muzzelFlash.isPlaying)
            {
                muzzelFlash.Stop();
            }
        }
    }
}
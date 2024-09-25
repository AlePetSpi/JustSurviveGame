using System.Collections;
using UnityEngine;


public class Missile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject startMissilePrefab;
    [SerializeField] private int secondsToDestroy = 9;

    [Header("MOVEMENT")]
    [SerializeField] private float speed = 15;
    [SerializeField] private float rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float maxDistancePredict = 100;
    [SerializeField] private float minDistancePredict = 5;
    [SerializeField] private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float deviationAmount = 50;
    [SerializeField] private float deviationSpeed = 2;
    [SerializeField] private int damage = 50;

    private Vehicle _vehicle;

    public Vehicle Vehicle { get => _vehicle; set => _vehicle = value; }

    private void Awake()
    {
        if (startMissilePrefab)
        {
            GameObject effect = Instantiate(startMissilePrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }

    private void FixedUpdate()
    {
        if (_vehicle == null) return;
        rb.velocity = transform.forward * speed;

        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, _vehicle.transform.position));

        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();

        StartCoroutine(DestroyMissle());
    }

    private IEnumerator DestroyMissle()
    {

        yield return new WaitForSeconds(secondsToDestroy);
        Destroy(gameObject);
        Debug.Log($"Missile destroyed");
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = _vehicle.Rb.position + _vehicle.Rb.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = null;
        if (explosionPrefab)
        {
            explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (collision.transform.TryGetComponent<Vehicle>(out var vehicle))
        {
            Player player = vehicle.GetComponentInParent<Player>();

            if (player != null)
            {
                player.Hit(damage);
            }
        }
        Destroy(gameObject);
        Destroy(explosion);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}
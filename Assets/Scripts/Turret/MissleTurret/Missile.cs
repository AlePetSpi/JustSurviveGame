using UnityEngine;


public class Missile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject startMisslePrefab;

    [Header("MOVEMENT")]
    [SerializeField] private float speed = 15;
    [SerializeField] private float rotateSpeed = 95;
    [SerializeField] private float rangeOfMissle = 100;

    [Header("PREDICTION")]
    [SerializeField] private float maxDistancePredict = 100;
    [SerializeField] private float minDistancePredict = 5;
    [SerializeField] private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float deviationAmount = 50;
    [SerializeField] private float deviationSpeed = 2;
    [SerializeField] private int damage = 50;

    private Player _player;

    public Player Player { get => _player; set => _player = value; }

    private void FixedUpdate()
    {
        if (_player == null) return;
        rb.velocity = transform.forward * speed;

        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, _player.transform.position));

        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();

        if (Vector3.Distance(rb.velocity, rb.position) > rangeOfMissle)
        {
            Destroy(gameObject);
        }
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = _player.Rb.position + _player.Rb.velocity * predictionTime;
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
        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        if (collision.transform.TryGetComponent<IExplode>(out var ex)) ex.Hit(damage);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}
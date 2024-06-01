using UnityEngine;

public class ArcProjectile : EnemyProjectile
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    protected override void Start()
    {
        base.Start();
        rb.velocity = CalculateLaunchVelocity() * speed;
    }

    private Vector3 CalculateLaunchVelocity()
    {
        float angle = 20f;
        float radianAngle = angle * Mathf.Deg2Rad;

        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = (targetPosition - transform.position).magnitude;

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radianAngle));
        Vector3 launchVelocity = new Vector3(direction.x * velocity * Mathf.Cos(radianAngle), velocity * Mathf.Sin(radianAngle), direction.z * velocity * Mathf.Cos(radianAngle));

        return launchVelocity;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}

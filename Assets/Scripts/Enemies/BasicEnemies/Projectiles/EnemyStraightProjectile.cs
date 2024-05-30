using UnityEngine;

public class StraightProjectile : EnemyProjectile
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected override void Start()
    {
        base.Start();
        rb.velocity = transform.forward * speed;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}

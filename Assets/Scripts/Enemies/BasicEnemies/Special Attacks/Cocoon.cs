using UnityEngine;
using System.Collections;

public class Cocoon : MonoBehaviour
{
    [SerializeField] private int cocoonMaxHp;
    [SerializeField] private int enemiesSpawnQuantity;
    [SerializeField] private float timeBeforeHatch;
    [SerializeField] private GameObject enemyPrefab;

    private int cocoonCurHp;
    private bool isHatching = false;
    private bool isDestroyed = false;
    private Transform player;

    private void Start()
    {
        cocoonCurHp = cocoonMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(HatchTimer());
        PositionOnGround();
    }

    private void Update()
    {
        FacePlayer();
    }

    private IEnumerator HatchTimer()
    {
        yield return new WaitForSeconds(timeBeforeHatch);

        if (!isHatching && !isDestroyed)
            Hatch();
    }

    public void TakeDamage(int damage)
    {
        cocoonCurHp -= damage;

        if (cocoonCurHp <= 0 && !isDestroyed)
            Die();
    }

    private void Die()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }

    private void Hatch()
    {
        isHatching = true;

        for (int i = 0; i < enemiesSpawnQuantity; i++)
            Instantiate(enemyPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void FacePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }

    private void PositionOnGround()
    {
        // Ensure the Cocoon is positioned on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
        else
        {
            Debug.LogWarning("Cocoon could not find the ground.");
        }
    }
}

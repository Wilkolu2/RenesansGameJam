using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Player player;
    private WaveManager waveManager;

    private void Start()
    {
        player = GetComponent<Player>();
        waveManager = FindObjectOfType<WaveManager>();

        if (player != null)
            player.SetPlayerCurHp(player.GetPlayerMaxHp());
        else
            Debug.LogError("Player component not found on PlayerHealth object.");

        if (waveManager == null)
        {
            Debug.LogError("WaveManager not found on PlayerHealth object.");
        }
    }

    public void TakeDamage(int damage)
    {
        if (player != null)
        {
            player.ModifyPlayerHp(-damage);
            Debug.Log("Player health: " + player.GetPlayerCurHp());

            if (player.GetPlayerCurHp() <= 0)
                Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        GameManager.instance.OnPlayerDeath();
    }
}

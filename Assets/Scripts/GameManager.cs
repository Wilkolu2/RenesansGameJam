using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private string nextArenaSceneName;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OnPlayerDeath()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            if (player.HasSpareLife())
            {
                player.LoseSpareLife();
                SceneManager.LoadScene(nextArenaSceneName);
                StartCoroutine(WaitForSceneLoad(player));
            }
            else
            {
                OnPlayerDeathPermanent();
            }
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    public void OnPlayerDeathPermanent()
    {
        Debug.Log("Player has died permanently. Game Over.");
        // Implement game over logic here
        // e.g., SceneManager.LoadScene(gameOverSceneName);
    }

    private IEnumerator WaitForSceneLoad(Player player)
    {
        yield return new WaitForSeconds(1f);  // Wait a bit for the scene to load

        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.OnPlayerDeath();
        }
        else
        {
            Debug.LogError("WaveManager not found after scene load!");
        }

        player.ResetPlayer();
    }
}

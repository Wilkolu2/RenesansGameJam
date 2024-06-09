using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private string nextArenaSceneName;
    [SerializeField] private string menuSceneName;

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
            player.ChangeWeaponOnDeath();

            if (player.HasSpareLife())
            {
                player.LoseSpareLife();
                SceneManager.LoadScene(nextArenaSceneName);
                StartCoroutine(WaitForSceneLoad(player));
            }
            else
                OnPlayerDeathPermanent();
        }
    }

    public void OnPlayerDeathPermanent()
    {
        SceneManager.LoadScene(menuSceneName);

        Player player = FindObjectOfType<Player>();
        if (player != null)
            Destroy(player.gameObject);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator WaitForSceneLoad(Player player)
    {
        yield return new WaitForSeconds(1f);

        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
            waveManager.OnPlayerDeath();
        else
            Debug.LogError("WaveManager not found after scene load!");

        player.ResetPlayer();
        player.AttachWeaponModel();
    }
}

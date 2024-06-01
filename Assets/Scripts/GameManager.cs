using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(nextArenaSceneName);
    }
}

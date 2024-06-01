using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        bool isPaused = pauseMenu.activeSelf;
        pauseMenu.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1 : 0;
        Cursor.lockState = isPaused ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isPaused;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    //[SerializeField] private GameObject optionsPanel;
    //[SerializeField] private Slider volumeSlider;

    private void Start()
    {
        //optionsPanel.SetActive(false);
        //volumeSlider.value = AudioListener.volume;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCredits()
    {

    }

    //public void ShowOptions()
    //{
        //optionsPanel.SetActive(true);
    //}

    //public void AdjustVolume(float volume)
    //{
        //AudioListener.volume = volume;
    //}

    //public void CloseOptions()
    //{
        //optionsPanel.SetActive(false);
    //}

    public void QuitGame()
    {
        Application.Quit();
    }
}

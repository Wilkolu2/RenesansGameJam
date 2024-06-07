using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image spareLifeIndicator;

    private Player player;
    private WaveManager waveManager;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        waveManager = FindObjectOfType<WaveManager>();
    }

    private void Update()
    {
        if (player != null)
        {
            UpdateHealthBar();
            UpdateSpareLifeIndicator();
        }

        if (waveManager != null)
            UpdateWaveText();
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = (float)player.GetPlayerCurHp() / player.GetPlayerMaxHp();
        healthBarFill.fillAmount = healthPercentage;
    }

    private void UpdateSpareLifeIndicator()
    {
        spareLifeIndicator.enabled = player.HasSpareLife();
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + waveManager.GetCurrentWaveIndex();
    }
}

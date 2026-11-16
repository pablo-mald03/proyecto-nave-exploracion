using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using NaveExploracion.Data;

public class RobotBatteryTimer : MonoBehaviour
{
    [Header("Time configuration (3 min = 180s)")]
    public float maxBatteryTime = 180f;
    private float currentTime;
    private bool isBatteryDead = false;

    [Header("UI")]
    public TextMeshProUGUI batteryText;
    public GameObject gameOverPanel;

    [Header("Block when counts make 0")]
    public MonoBehaviour[] scriptsToDisable;
    public GameObject mobileControlsHUD;

    [Header("Extra event")]
    public UnityEvent onBatteryEmpty;

    void Start()
    {
        currentTime = maxBatteryTime;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isBatteryDead) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isBatteryDead = true;
            UpdateBatteryUI();
            TriggerBatteryDepleted();
            return;
        }

        UpdateBatteryUI();
    }

    private void UpdateBatteryUI()
    {
        if (batteryText == null) return;

        int percentage = Mathf.CeilToInt((currentTime / maxBatteryTime) * 100f);
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        batteryText.text = $"Battery: {percentage}% ({minutes:00}:{seconds:00})";

        if (percentage <= 20)
        {
            batteryText.color = Color.red;
        }
    }

    private IEnumerator LoadGameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOver");
    }

    private void TriggerBatteryDepleted()
    {
        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = false;
        }

        if (mobileControlsHUD != null)
        {
            mobileControlsHUD.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        onBatteryEmpty?.Invoke();
        MissionSaveManager.SetLastMissionScene(SceneManager.GetActiveScene().name);
        StartCoroutine(LoadGameOverRoutine());
    }


    public void RetryMission()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void ReturnToBaseWithoutCompleting()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base_Scene");
    }
}
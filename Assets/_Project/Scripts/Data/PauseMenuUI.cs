using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaveExploracion.Data;

namespace NaveExploracion.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject pauseModalContainer;
        [SerializeField] private GameObject gameplayHudGroup;

        [Header("Feedback Texts")]
        [SerializeField] private TextMeshProUGUI missionsCounterText;
        [SerializeField] private TextMeshProUGUI saveFeedbackText;

        [Header("Spawn & Player References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform defaultSpawnPoint;

        [Header("Scene Navigation")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        //Method to find the spawn for the player
        private void Start()
        {
            Time.timeScale = 1f;

            if (pauseModalContainer != null)
                pauseModalContainer.SetActive(false);

            if (playerTransform == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) playerTransform = playerObj.transform;
            }

            MissionSaveManager.ApplySavedSpawn(playerTransform, defaultSpawnPoint);
        }

        public void OpenPauseMenu()
        {
            Time.timeScale = 0f;

            if (gameplayHudGroup != null)
                gameplayHudGroup.SetActive(false);

            if (missionsCounterText != null)
                missionsCounterText.text = $"MISSIONS COMPLETED: {MissionSaveManager.GetCompletedMissionsCount()}";

            if (saveFeedbackText != null)
                saveFeedbackText.text = string.Empty;

            if (pauseModalContainer != null)
                pauseModalContainer.SetActive(true);
        }

        public void ResumeGame()
        {
            if (pauseModalContainer != null)
                pauseModalContainer.SetActive(false);

            if (gameplayHudGroup != null)
                gameplayHudGroup.SetActive(true);

            Time.timeScale = 1f;
        }

        public void SaveGameProgress()
        {
            MissionSaveManager.SaveCheckpoint(playerTransform);

            if (saveFeedbackText != null)
                saveFeedbackText.text = "PROGRESS SAVED!";
        }

        public void ExitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
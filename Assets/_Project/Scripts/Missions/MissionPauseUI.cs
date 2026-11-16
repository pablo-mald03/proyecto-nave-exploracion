using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NaveExploracion.UI
{
    public class MissionPauseUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject pauseModal;
        [SerializeField] private GameObject gameplayHudGroup;

        [Header("Buttons")]
        [SerializeField] private Button pauseTriggerButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button retryMissionButton;
        [SerializeField] private Button returnToBaseButton;

        [Header("Scene Settings")]
        [SerializeField] private string baseSceneName = "Base_Scene";

        private bool _isPaused;

        private void Awake()
        {
            if (pauseModal != null)
                pauseModal.SetActive(false);

            if (pauseTriggerButton != null)
                pauseTriggerButton.onClick.AddListener(PauseMission);

            if (resumeButton != null)
                resumeButton.onClick.AddListener(ResumeMission);

            if (retryMissionButton != null)
                retryMissionButton.onClick.AddListener(RetryMission);

            if (returnToBaseButton != null)
                returnToBaseButton.onClick.AddListener(ReturnToBase);
        }

        private void OnDestroy()
        {
            // Seguridad: asegura que el tiempo nunca se quede congelado al cambiar de escena
            Time.timeScale = 1f;
        }

        public void PauseMission()
        {
            if (_isPaused) return;
            _isPaused = true;

            Time.timeScale = 0f; // Congela el temporizador de batería, físicas y movimiento

            if (gameplayHudGroup != null)
                gameplayHudGroup.SetActive(false);

            if (pauseModal != null)
                pauseModal.SetActive(true);
        }

        public void ResumeMission()
        {
            if (!_isPaused) return;
            _isPaused = false;

            Time.timeScale = 1f; // Reanuda el tiempo normal

            if (pauseModal != null)
                pauseModal.SetActive(false);

            if (gameplayHudGroup != null)
                gameplayHudGroup.SetActive(true);
        }

        public void RetryMission()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReturnToBase()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(baseSceneName);
        }
    }
}
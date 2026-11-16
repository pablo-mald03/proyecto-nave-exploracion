using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaveExploracion.Data;

namespace NaveExploracion.UI
{
    public class MissionModalUI : MonoBehaviour
    {
        public static MissionModalUI Instance { get; private set; }

        [Header("UI Panels")]
        [SerializeField] private GameObject missionModalContainer;
        [SerializeField] private GameObject gameplayHudGroup;

        [Header("Texts (Hussar Bold)")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statusBadgeText;
        [SerializeField] private TextMeshProUGUI briefingText;

        [Header("Action Buttons")]
        [SerializeField] private Button launchMissionButton;
        [SerializeField] private Button cancelButton;

        [Header("Typewriter Settings")]
        [SerializeField] private float charDelay = 0.02f;

        private MissionEntry _currentMission;
        private Transform _playerTransform;
        private Coroutine _typingCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (missionModalContainer != null)
                missionModalContainer.SetActive(false);

            if (launchMissionButton != null)
                launchMissionButton.onClick.AddListener(LaunchMissionScene);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(CloseModal);
        }

        public void OpenMission(string missionId, Transform playerTransform)
        {
            _currentMission = MissionDatabase.GetMission(missionId);
            _playerTransform = playerTransform;

            bool isCompleted = MissionSaveManager.IsMissionCompleted(_currentMission.MissionId);

            if (titleText != null)
                titleText.text = _currentMission.Title;

            if (statusBadgeText != null)
            {
                statusBadgeText.text = isCompleted ? "ESTADO: COMPLETADA" : "ESTADO: PENDIENTE";
                statusBadgeText.color = isCompleted ? new Color(0.29f, 0.87f, 0.5f) : new Color(1f, 0.82f, 0f);
            }

            if (gameplayHudGroup != null)
                gameplayHudGroup.SetActive(false);

            if (missionModalContainer != null)
                missionModalContainer.SetActive(true);

            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            _typingCoroutine = StartCoroutine(TypeBriefingRoutine(_currentMission.BriefingText));
        }

        private IEnumerator TypeBriefingRoutine(string fullText)
        {
            if (briefingText == null) yield break;

            briefingText.text = string.Empty;
            foreach (char c in fullText)
            {
                briefingText.text += c;
                yield return new WaitForSecondsRealtime(charDelay);
            }
        }

        public void LaunchMissionScene()
        {
            if (string.IsNullOrEmpty(_currentMission.TargetSceneName)) return;

            // Auto-save player spawn right before leaving Base_Scene
            if (_playerTransform == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) _playerTransform = playerObj.transform;
            }
            MissionSaveManager.SaveCheckpoint(_playerTransform);

            Time.timeScale = 1f;
            MissionSaveManager.SetLastMissionScene(_currentMission.TargetSceneName);
            SceneManager.LoadScene(_currentMission.TargetSceneName);
        }

        public void CloseModal()
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            if (missionModalContainer != null)
                missionModalContainer.SetActive(false);

            if (gameplayHudGroup != null)
                gameplayHudGroup.SetActive(true);
        }
    }
}
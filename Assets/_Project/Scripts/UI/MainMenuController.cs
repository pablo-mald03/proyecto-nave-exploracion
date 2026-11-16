using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaveExploracion.Data;

namespace NaveExploracion.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newMissionButton;

        [Header("Fade Transition")]
        [SerializeField] private CanvasGroup fadeOverlayGroup;
        [SerializeField] private float fadeDuration = 0.85f;
        [SerializeField] private string gameplaySceneName = "Base_Scene";

        private bool _isTransitioning;

        private void Start()
        {
            Time.timeScale = 1f;

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(MissionSaveManager.HasSavedGame());
                continueButton.onClick.AddListener(OnContinuePressed);
            }

            if (newMissionButton != null)
            {
                newMissionButton.onClick.AddListener(OnNewMissionPressed);
            }

            if (fadeOverlayGroup != null)
            {
                fadeOverlayGroup.alpha = 1f;
                fadeOverlayGroup.blocksRaycasts = false;
                StartCoroutine(FadeRoutine(1f, 0f, null));
            }
        }

        private void OnContinuePressed()
        {
            if (_isTransitioning) return;
            StartTransitionToGame();
        }

        private void OnNewMissionPressed()
        {
            if (_isTransitioning) return;
            MissionSaveManager.ResetSaveData();
            StartTransitionToGame();
        }

        private void StartTransitionToGame()
        {
            _isTransitioning = true;
            if (fadeOverlayGroup != null)
            {
                fadeOverlayGroup.blocksRaycasts = true;
                StartCoroutine(FadeRoutine(0f, 1f, () => SceneManager.LoadScene(gameplaySceneName)));
            }
            else
            {
                SceneManager.LoadScene(gameplaySceneName);
            }
        }

        private IEnumerator FadeRoutine(float fromAlpha, float toAlpha, System.Action onComplete)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeOverlayGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeDuration);
                yield return null;
            }
            fadeOverlayGroup.alpha = toAlpha;
            onComplete?.Invoke();
        }
    }
}
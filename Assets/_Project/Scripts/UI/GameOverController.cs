using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaveExploracion.Data;

namespace NaveExploracion.UI
{
    public class GameOverController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button retryMissionButton;
        [SerializeField] private Button returnToBaseButton;

        [Header("Fade Transition")]
        [SerializeField] private CanvasGroup fadeOverlayGroup;
        [SerializeField] private float fadeDuration = 0.85f;
        [SerializeField] private string baseSceneName = "Base_Scene";

        private bool _isTransitioning;

        private void Start()
        {
            Time.timeScale = 1f;

            if (retryMissionButton != null)
                retryMissionButton.onClick.AddListener(OnRetryPressed);

            if (returnToBaseButton != null)
                returnToBaseButton.onClick.AddListener(OnReturnToBasePressed);

            // Fade-in suave al entrar a Game Over
            if (fadeOverlayGroup != null)
            {
                fadeOverlayGroup.alpha = 1f;
                fadeOverlayGroup.blocksRaycasts = false;
                StartCoroutine(FadeRoutine(1f, 0f, null));
            }
        }

        private void OnRetryPressed()
        {
            if (_isTransitioning) return;
            string missionScene = MissionSaveManager.GetLastMissionScene();
            StartTransition(missionScene);
        }

        private void OnReturnToBasePressed()
        {
            if (_isTransitioning) return;
            StartTransition(baseSceneName);
        }

        private void StartTransition(string targetScene)
        {
            _isTransitioning = true;
            if (fadeOverlayGroup != null)
            {
                fadeOverlayGroup.blocksRaycasts = true;
                StartCoroutine(FadeRoutine(0f, 1f, () => SceneManager.LoadScene(targetScene)));
            }
            else
            {
                SceneManager.LoadScene(targetScene);
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
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NaveExploracion.UI
{
    public class SceneFader : MonoBehaviour
    {
        public static SceneFader Instance { get; private set; }

        [Header("Fade Overlay")]
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private float defaultFadeDuration = 0.65f;

        private bool _isFading;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (fadeCanvasGroup != null)
            {
                StopAllCoroutines();
                _isFading = false;
                fadeCanvasGroup.alpha = 1f;
                fadeCanvasGroup.blocksRaycasts = false;
                StartCoroutine(FadeRoutine(1f, 0f, defaultFadeDuration, null));
            }
        }

        public void LoadSceneWithFade(string sceneName, float duration = -1f)
        {
            if (_isFading) return;

            float fadeTime = duration > 0f ? duration : defaultFadeDuration;

            if (fadeCanvasGroup == null)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(sceneName);
                return;
            }

            StartCoroutine(TransitionRoutine(sceneName, fadeTime));
        }

        private IEnumerator TransitionRoutine(string sceneName, float fadeTime)
        {
            _isFading = true;
            fadeCanvasGroup.blocksRaycasts = true;

            yield return FadeRoutine(fadeCanvasGroup.alpha, 1f, fadeTime, null);

            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }

        private IEnumerator FadeRoutine(float fromAlpha, float toAlpha, float duration, System.Action onComplete)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
                yield return null;
            }

            fadeCanvasGroup.alpha = toAlpha;
            onComplete?.Invoke();
        }
    }
}
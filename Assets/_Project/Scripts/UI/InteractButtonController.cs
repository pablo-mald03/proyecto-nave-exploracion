using UnityEngine;
using UnityEngine.UI;
using NaveExploracion.Gameplay;

namespace NaveExploracion.UI
{
    [RequireComponent(typeof(Button))]
    public class InteractButtonController : MonoBehaviour
    {
        public static InteractButtonController Instance { get; private set; }

        [Header("Visual Feedback")]
        [SerializeField] private CanvasGroup buttonCanvasGroup;
        [SerializeField] private float inactiveAlpha = 0.35f;
        [SerializeField] private float pulseSpeed = 5f;
        [SerializeField] private float pulseScaleAmount = 0.12f;

        private Button _button;
        private InteractableStationPoint _currentTarget;
        private Vector3 _baseScale;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _button = GetComponent<Button>();
            _baseScale = transform.localScale;
            _button.onClick.AddListener(HandleInteractPressed);

            SetTarget(null);
        }

        private void Update()
        {
            if (_currentTarget == null)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _baseScale, Time.unscaledDeltaTime * 10f);
                return;
            }

            float wave = 1f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseScaleAmount;
            transform.localScale = _baseScale * wave;
        }

        public void SetTarget(InteractableStationPoint target)
        {
            _currentTarget = target;
            bool hasTarget = _currentTarget != null;

            _button.interactable = hasTarget;
            if (buttonCanvasGroup != null)
            {
                buttonCanvasGroup.alpha = hasTarget ? 1f : inactiveAlpha;
            }
        }

        public void ClearTargetIfMatches(InteractableStationPoint target)
        {
            if (_currentTarget == target)
            {
                SetTarget(null);
            }
        }

        private void HandleInteractPressed()
        {
            if (_currentTarget != null)
            {
                _currentTarget.Interact();
            }
        }
    }
}
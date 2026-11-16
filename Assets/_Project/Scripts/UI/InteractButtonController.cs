using UnityEngine;
using UnityEngine.UI;
using NaveExploracion.Gameplay;

namespace NaveExploracion.UI
{
    [RequireComponent(typeof(Button))]
    public class InteractButtonController : MonoBehaviour
    {
        public static InteractButtonController Instance { get; private set; }

        [Header("Visual Feedback (Brawl Stars Pulse)")]
        [SerializeField] private CanvasGroup buttonCanvasGroup;
        [SerializeField] private float inactiveAlpha = 0.35f;
        [SerializeField] private float pulseSpeed = 5f;
        [SerializeField] private float pulseScaleAmount = 0.12f;

        private Button _button;
        private InteractableStationPoint _dialogueTarget;
        private MissionStationPoint _missionTarget;
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

            RefreshVisualState();
        }

        private void Update()
        {
            bool hasAnyTarget = _dialogueTarget != null || _missionTarget != null;
            if (!hasAnyTarget)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _baseScale, Time.unscaledDeltaTime * 10f);
                return;
            }

            float wave = 1f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseScaleAmount;
            transform.localScale = _baseScale * wave;
        }

        public void SetTarget(InteractableStationPoint target)
        {
            _dialogueTarget = target;
            if (target != null) _missionTarget = null;
            RefreshVisualState();
        }

        public void ClearTargetIfMatches(InteractableStationPoint target)
        {
            if (_dialogueTarget == target)
            {
                _dialogueTarget = null;
                RefreshVisualState();
            }
        }

        public void SetMissionTarget(MissionStationPoint target)
        {
            _missionTarget = target;
            if (target != null) _dialogueTarget = null;
            RefreshVisualState();
        }

        public void ClearMissionTargetIfMatches(MissionStationPoint target)
        {
            if (_missionTarget == target)
            {
                _missionTarget = null;
                RefreshVisualState();
            }
        }

        private void RefreshVisualState()
        {
            bool hasTarget = _dialogueTarget != null || _missionTarget != null;
            _button.interactable = hasTarget;
            if (buttonCanvasGroup != null)
                buttonCanvasGroup.alpha = hasTarget ? 1f : inactiveAlpha;
        }

        private void HandleInteractPressed()
        {
            if (_missionTarget != null)
            {
                _missionTarget.Interact();
            }
            else if (_dialogueTarget != null)
            {
                _dialogueTarget.Interact();
            }
        }
    }
}
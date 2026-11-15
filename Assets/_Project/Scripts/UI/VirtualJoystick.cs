using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace SpaceExplorer.UI
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("Joystick Visuals")]
        [SerializeField] private RectTransform backgroundRing;
        [SerializeField] private RectTransform handleKnob;

        [Header("Settings")]
        [SerializeField] private float handleRange = 80f;

        [Header("Output Event")]
        public UnityEvent<Vector2> onJoystickMoved;

        private Vector2 _inputVector;

        private void Start()
        {
            if (backgroundRing == null) backgroundRing = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                backgroundRing, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                Vector2 normalizedPos = new Vector2(
                    localPoint.x / (backgroundRing.sizeDelta.x * 0.5f),
                    localPoint.y / (backgroundRing.sizeDelta.y * 0.5f)
                );

                _inputVector = Vector2.ClampMagnitude(normalizedPos, 1f);

                if (handleKnob != null)
                {
                    handleKnob.anchoredPosition = _inputVector * handleRange;
                }

                onJoystickMoved?.Invoke(_inputVector);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputVector = Vector2.zero;

            if (handleKnob != null)
            {
                handleKnob.anchoredPosition = Vector2.zero;
            }

            onJoystickMoved?.Invoke(Vector2.zero);
        }
    }
}
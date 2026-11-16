using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NaveExploracion.UI
{
    public class HollowButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Side Flourishes / Sparkles")]
        [SerializeField] private RectTransform leftFlourish;
        [SerializeField] private RectTransform rightFlourish;
        [SerializeField] private CanvasGroup flourishesGroup;

        [Header("Label Styling")]
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Color normalColor = new Color(0.85f, 0.9f, 0.95f, 0.85f);
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private float expandOffset = 18f;
        [SerializeField] private float animSpeed = 12f;

        private bool _isHighlighted;
        private Vector2 _leftBasePos;
        private Vector2 _rightBasePos;

        private void Awake()
        {
            if (leftFlourish != null) _leftBasePos = leftFlourish.anchoredPosition;
            if (rightFlourish != null) _rightBasePos = rightFlourish.anchoredPosition;
            if (flourishesGroup != null) flourishesGroup.alpha = 0f;
            if (labelText != null) labelText.color = normalColor;
        }

        private void Update()
        {
            float targetAlpha = _isHighlighted ? 1f : 0f;
            float targetOffset = _isHighlighted ? expandOffset : 0f;

            if (flourishesGroup != null)
            {
                flourishesGroup.alpha = Mathf.Lerp(flourishesGroup.alpha, targetAlpha, Time.deltaTime * animSpeed);
            }

            if (leftFlourish != null)
            {
                Vector2 targetL = _leftBasePos + Vector2.left * targetOffset;
                leftFlourish.anchoredPosition = Vector2.Lerp(leftFlourish.anchoredPosition, targetL, Time.deltaTime * animSpeed);
            }

            if (rightFlourish != null)
            {
                Vector2 targetR = _rightBasePos + Vector2.right * targetOffset;
                rightFlourish.anchoredPosition = Vector2.Lerp(rightFlourish.anchoredPosition, targetR, Time.deltaTime * animSpeed);
            }

            if (labelText != null)
            {
                Color targetCol = _isHighlighted ? activeColor : normalColor;
                labelText.color = Color.Lerp(labelText.color, targetCol, Time.deltaTime * animSpeed);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => _isHighlighted = true;
        public void OnPointerExit(PointerEventData eventData) => _isHighlighted = false;
        public void OnPointerDown(PointerEventData eventData) => _isHighlighted = true;
        public void OnPointerUp(PointerEventData eventData) => _isHighlighted = false;
    }
}
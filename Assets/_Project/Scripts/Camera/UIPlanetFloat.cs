using UnityEngine;

//Planet animation
public class UIPlanetFloat : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -2f;
    [SerializeField] private float bobAmplitude = 12f;
    [SerializeField] private float bobFrequency = 0.6f;

    private RectTransform _rectTransform;
    private Vector2 _startAnchoredPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startAnchoredPos = _rectTransform.anchoredPosition;
    }

    private void Update()
    {
        _rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        float offsetY = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        _rectTransform.anchoredPosition = _startAnchoredPos + new Vector2(0f, offsetY);
    }
}
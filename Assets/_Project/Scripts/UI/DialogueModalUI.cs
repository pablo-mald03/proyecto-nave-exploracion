using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaveExploracion.Data;

namespace NaveExploracion.UI
{
    //Principal dialogue modal component
    public class DialogueModalUI : MonoBehaviour, IPointerClickHandler
    {
        public static DialogueModalUI Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private GameObject modalContainer;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI bodyText;
        [SerializeField] private TextMeshProUGUI tapHintText;

        [Header("Typing Settings")]
        [SerializeField] private float secondsPerCharacter = 0.025f;

        [Header("Optional Gameplay Lock")]
        [SerializeField] private GameObject mobileControlsGroup;

        private string[] _currentPages;
        private int _currentPageIndex;
        private bool _isTyping;
        private Coroutine _typingCoroutine;

        public bool IsOpen => modalContainer != null && modalContainer.activeSelf;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (modalContainer != null)
                modalContainer.SetActive(false);
        }

        public void OpenTopic(string topicId)
        {
            EducationalTopic topic = StationDialogueDatabase.GetTopic(topicId);

            if (titleText != null)
                titleText.text = topic.Title;

            _currentPages = topic.Pages;
            _currentPageIndex = 0;

            if (mobileControlsGroup != null)
                mobileControlsGroup.SetActive(false);

            modalContainer.SetActive(true);
            ShowCurrentPage();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsOpen) return;

            if (_isTyping)
            {
                if (_typingCoroutine != null)
                    StopCoroutine(_typingCoroutine);

                bodyText.text = _currentPages[_currentPageIndex];
                _isTyping = false;
                UpdateTapHint();
            }
            else
            {
                // Next tap goes to next page or closes modal
                _currentPageIndex++;
                if (_currentPageIndex < _currentPages.Length)
                {
                    ShowCurrentPage();
                }
                else
                {
                    CloseModal();
                }
            }
        }

        private void ShowCurrentPage()
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            _typingCoroutine = StartCoroutine(TypeTextRoutine(_currentPages[_currentPageIndex]));
        }

        private IEnumerator TypeTextRoutine(string fullText)
        {
            _isTyping = true;
            bodyText.text = string.Empty;

            if (tapHintText != null)
                tapHintText.text = "TAP TO SKIP >>";

            foreach (char letter in fullText)
            {
                bodyText.text += letter;
                yield return new WaitForSecondsRealtime(secondsPerCharacter);
            }

            _isTyping = false;
            UpdateTapHint();
        }

        private void UpdateTapHint()
        {
            if (tapHintText == null) return;

            bool isLastPage = _currentPageIndex >= _currentPages.Length - 1;
            tapHintText.text = isLastPage
                ? $"TAP TO CLOSE ({_currentPageIndex + 1}/{_currentPages.Length})"
                : $"TAP TO CONTINUE ({_currentPageIndex + 1}/{_currentPages.Length}) ▼";
        }

        public void CloseModal()
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            _isTyping = false;
            modalContainer.SetActive(false);

            if (mobileControlsGroup != null)
                mobileControlsGroup.SetActive(true);
        }
    }
}
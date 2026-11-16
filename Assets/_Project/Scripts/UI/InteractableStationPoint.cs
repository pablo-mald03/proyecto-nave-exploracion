using UnityEngine;
using NaveExploracion.Data;
using NaveExploracion.UI;

namespace NaveExploracion.Gameplay
{
    [RequireComponent(typeof(SphereCollider))]
    public class InteractableStationPoint : MonoBehaviour
    {
        [Header("Educational Topic ID")]
        [Tooltip("Must match a key in StationDialogueDatabase")]
        [SerializeField] private string topicId = StationDialogueDatabase.TOPIC_CONSOLE_OXYGEN;

        [Header("Ground Beacon Ring")]
        [SerializeField] private Transform groundRingVisual;
        [SerializeField] private float ringPulseSpeed = 3f;
        [SerializeField] private float ringScaleAmplitude = 0.08f;

        private Vector3 _ringBaseScale;

        private void Awake()
        {
            SphereCollider trigger = GetComponent<SphereCollider>();
            trigger.isTrigger = true;

            if (groundRingVisual != null)
            {
                _ringBaseScale = groundRingVisual.localScale;
            }
        }

        private void Update()
        {
            if (groundRingVisual == null) return;

            float pulse = 1f + Mathf.Sin(Time.time * ringPulseSpeed) * ringScaleAmplitude;
            groundRingVisual.localScale = new Vector3(
                _ringBaseScale.x * pulse,
                _ringBaseScale.y,
                _ringBaseScale.z * pulse
            );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (InteractButtonController.Instance != null)
                {
                    InteractButtonController.Instance.SetTarget(this);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (InteractButtonController.Instance != null)
                {
                    InteractButtonController.Instance.ClearTargetIfMatches(this);
                }
            }
        }

        public void Interact()
        {
            if (DialogueModalUI.Instance != null)
            {
                DialogueModalUI.Instance.OpenTopic(topicId);
            }
        }
    }
}
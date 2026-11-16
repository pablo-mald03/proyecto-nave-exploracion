using UnityEngine;
using NaveExploracion.Data;
using NaveExploracion.UI;

namespace NaveExploracion.Gameplay
{
    [RequireComponent(typeof(SphereCollider))]
    public class MissionStationPoint : MonoBehaviour
    {
        [Header("Mission ID (MissionDatabase)")]
        [SerializeField] private string missionId = MissionDatabase.MISSION_01;

        [Header("Ground Beacon Ring")]
        [SerializeField] private Transform groundRingVisual;
        [SerializeField] private float ringPulseSpeed = 3f;
        [SerializeField] private float ringScaleAmplitude = 0.08f;

        private Vector3 _ringBaseScale;
        private Transform _nearbyPlayer;

        private void Awake()
        {
            SphereCollider trigger = GetComponent<SphereCollider>();
            trigger.isTrigger = true;

            if (groundRingVisual != null)
                _ringBaseScale = groundRingVisual.localScale;
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
                _nearbyPlayer = other.transform;
                if (InteractButtonController.Instance != null)
                    InteractButtonController.Instance.SetMissionTarget(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _nearbyPlayer = null;
                if (InteractButtonController.Instance != null)
                    InteractButtonController.Instance.ClearMissionTargetIfMatches(this);
            }
        }

        public void Interact()
        {
            if (MissionModalUI.Instance != null)
                MissionModalUI.Instance.OpenMission(missionId, _nearbyPlayer);
        }
    }
}
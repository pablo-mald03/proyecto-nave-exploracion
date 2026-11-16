using UnityEngine;
using UnityEngine.InputSystem;
using SpaceExplorer.Player;

namespace SpaceExplorer.CameraSystem
{
    public class DioramaCameraRig : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private float followSmoothness = 8f;

        [Header("Orbit Rotation (Touch Drag / Right Mouse)")]
        [SerializeField] private float rotationSensitivity = 0.25f;
        [SerializeField] private float rotationSmoothness = 12f;

        private float _targetYaw;
        private float _currentYaw;

        private void Start()
        {
            _currentYaw = transform.eulerAngles.y;
            _targetYaw = _currentYaw;

            if (target != null && target.TryGetComponent(out PlayerMovement playerMovement))
            {
                playerMovement.SetCameraReference(transform);
            }
        }

        private void LateUpdate()
        {
            HandleRotationInput();
            FollowAndRotate();
        }

        private void HandleRotationInput()
        {
            float deltaX = 0f;

            // Mobile Touch Drag 
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
                if (touchPos.x > Screen.width * 0.35f)
                {
                    deltaX = Touchscreen.current.primaryTouch.delta.ReadValue().x;
                }
            }
            // PC Testing 
            else if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
                deltaX = Mouse.current.delta.ReadValue().x;
            }

            if (Mathf.Abs(deltaX) > 0.01f)
            {
                _targetYaw += deltaX * rotationSensitivity;
            }
        }

        //Method to move the cam with the rotation integrated
        private void FollowAndRotate()
        {
            // Smoothly follow the player's position
            if (target != null)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, followSmoothness * Time.deltaTime);
            }

            // Smoothly rotate around the Y axis
            _currentYaw = Mathf.LerpAngle(_currentYaw, _targetYaw, rotationSmoothness * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, _currentYaw, 0f);
        }
    }
}
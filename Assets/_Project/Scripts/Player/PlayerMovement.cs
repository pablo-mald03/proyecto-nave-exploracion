using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceExplorer.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Camera Alignment")]
        [Tooltip("Reference to the CameraRig or Main Camera so movement aligns with what the player sees.")]
        [SerializeField] private Transform cameraTransform;

        private CharacterController _controller;
        private Vector2 _externalMoveInput;
        private float _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector2 input = GetCombinedInput();
            MoveCharacter(input);
        }

        // Method to allows external systems to assign the reference transform cleanly.
        public void SetCameraReference(Transform camTransform)
        {
            cameraTransform = camTransform;
        }

        // Method for the future mobile Virtual Joystick to inject movement values.
        public void SetMoveInput(Vector2 moveInput)
        {
            _externalMoveInput = Vector2.ClampMagnitude(moveInput, 1f);
        }

        private Vector2 GetCombinedInput()
        {
            Vector2 keyboardInput = Vector2.zero;

            if (Keyboard.current != null)
            {
                float x = 0f;
                float y = 0f;

                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y -= 1f;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y += 1f;

                keyboardInput = Vector2.ClampMagnitude(new Vector2(x, y), 1f);
            }

            return _externalMoveInput.sqrMagnitude > 0.01f ? _externalMoveInput : keyboardInput;
        }

        private void MoveCharacter(Vector2 input)
        {
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;

            if (cameraTransform != null)
            {
                forward = cameraTransform.forward;
                right = cameraTransform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
            }

            Vector3 moveDirection = (forward * input.y + right * input.x).normalized;

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }
            _verticalVelocity += gravity * Time.deltaTime;

            Vector3 finalVelocity = (moveDirection * moveSpeed) + (Vector3.up * _verticalVelocity);
            _controller.Move(finalVelocity * Time.deltaTime);
        }
    }
}
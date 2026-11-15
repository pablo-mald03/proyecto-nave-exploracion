using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceExplorer.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement & Jump Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -15f;

        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Animator characterAnimator;

        private CharacterController _controller;
        private Vector2 _externalMoveInput;
        private float _verticalVelocity;
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (characterAnimator == null)
            {
                characterAnimator = GetComponentInChildren<Animator>();
            }
        }

        private void Update()
        {
            Vector2 input = GetCombinedInput();
            HandleJumpInput();
            MoveCharacter(input);
        }

        public void SetCameraReference(Transform camTransform) => cameraTransform = camTransform;

        public void SetMoveInput(Vector2 moveInput) => _externalMoveInput = Vector2.ClampMagnitude(moveInput, 1f);

        /// Method for the mobile UI Jump Button to trigger a jump.
        public void TriggerJump()
        {
            if (_controller.isGrounded)
            {
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        private void HandleJumpInput()
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TriggerJump();
            }
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

        // Method to move the character
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

            Vector3 horizontalVelocity = moveDirection * moveSpeed;
            Vector3 finalVelocity = horizontalVelocity + (Vector3.up * _verticalVelocity);
            _controller.Move(finalVelocity * Time.deltaTime);

            // Feed movement speed to Animator 
            if (characterAnimator != null && characterAnimator.runtimeAnimatorController != null)
            {
                float currentSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;
                characterAnimator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    public Transform playerCamera;
    public float touchSensitivity = 0.25f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        yRotation = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        Vector2 lookDelta = Vector2.zero;

        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.isPressed && touch.position.ReadValue().x > Screen.width * 0.5f)
                {
                    lookDelta = touch.delta.ReadValue() * touchSensitivity;
                    break;
                }
            }
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed && Mouse.current.position.ReadValue().x > Screen.width * 0.5f)
        {
            lookDelta = Mouse.current.delta.ReadValue() * touchSensitivity;
        }

        yRotation += lookDelta.x;
        xRotation -= lookDelta.y;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public Transform playerCamera;
    public float joystickSensitivity = 120f;

    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        yRotation = transform.eulerAngles.y;
    }

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    void LateUpdate()
    {
        yRotation += lookInput.x * joystickSensitivity * Time.deltaTime;
        xRotation -= lookInput.y * joystickSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
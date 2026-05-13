using UnityEngine;

public class MobileAndPCMouseLook : MonoBehaviour
{
    public float sensibilidade = 100f;
    public Transform playerBody;
    public Transform cameraHolder;

    float xRotation = 0f;

    void Start()
    {
        if (!Application.isMobilePlatform)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                Touch toque = Input.GetTouch(0);
                if (toque.phase == TouchPhase.Moved)
                {
                    mouseX = toque.deltaPosition.x * sensibilidade * 0.001f;
                    mouseY = toque.deltaPosition.y * sensibilidade * 0.001f;
                }
            }
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * sensibilidade * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensibilidade * Time.deltaTime;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Transform pitchTarget = cameraHolder != null ? cameraHolder : transform;
        pitchTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }
}
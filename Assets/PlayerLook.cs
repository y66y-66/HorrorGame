using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform cameraHolder;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // プレイヤー自体の横回転（左右）
        transform.Rotate(Vector3.up * mouseX);

        // カメラの縦回転（上下）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

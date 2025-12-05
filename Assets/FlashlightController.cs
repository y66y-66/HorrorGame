using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;   // Spot Light を入れる
    public KeyCode toggleKey = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceFilter : MonoBehaviour
{
    void Awake()
    {
        foreach (var device in InputSystem.devices)
        {
            Debug.Log("Found Device: " + device.name);
            if (device.name.Contains("Nintendo"))
            {
                InputSystem.DisableDevice(device);
                Debug.Log("Đã chặn thiết bị nhiễu: " + device.name);
            }
        }
    }
}

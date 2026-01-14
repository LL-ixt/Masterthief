using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat2 : PlayerCombat
{
    [Header("Cấu hình Khiên Level 2+3")]
    public GameObject shieldObject; 
    public bool isShielding = false;

    protected override void Update()
    {
        HandleShieldInput();

        if (isShielding) return; 

        base.Update();
    }

    void HandleShieldInput()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !isShielding && ScoreManager.Instance.CheckScore(5f))
        {
            ScoreManager.Instance.UpdateScore(-5f);
            isShielding = true;
            if (shieldObject != null)
            {
                shieldObject.SetActive(true);
                RotateShieldToMouse(); // Gọi hàm xoay khiên
            }
        }
        else if (isShielding && Mouse.current.rightButton.wasPressedThisFrame)
        {
            isShielding = false;
            if (shieldObject != null) shieldObject.SetActive(false);
        }
        else if (isShielding)
        {
            RotateShieldToMouse();
        }
    }

    void RotateShieldToMouse()
    {
        // 1. Lấy vị trí chuột từ Input System
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        
        // 2. Chuyển đổi vị trí chuột sang tọa độ World
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));
        mouseWorldPos.z = 0f; // Đảm bảo luôn nằm trên mặt phẳng 2D

        // 3. Tính toán hướng từ Player tới chuột, chú ý z = 0 tương ứng với direction = (0, -1);
        Vector2 direction = (mouseWorldPos - shieldObject.transform.position).normalized;
        Debug.Log(direction);
        // 4. Tính toán góc xoay (Angle)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        // 5. Áp dụng góc xoay vào Shield Object
        // Giả sử mặt trước của khiên là trục X (phía bên phải của Sprite)
        shieldObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
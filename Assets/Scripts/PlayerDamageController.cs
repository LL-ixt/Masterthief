using UnityEngine;
using UnityEngine.UI;
public class PlayerDamageController : MonoBehaviour, IDamageable
{
    private const float maxHP = 100f;
    public float hp = 100f;
    public float laserDamage = 10f;
    public Image hpFill;
    public Image energyFIll;
    public GameObject hpWarning;
    public PlayerController playerController;
    private bool hasPlayedDie = false;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReceiveDamage(float damageAmount)
    {
        hp -= damageAmount;
        
        // Đảm bảo HP luôn nằm trong khoảng 0 đến 100
        hp = Mathf.Clamp(hp, 0, 100f);

        // Xử lý hiển thị cảnh báo HP thấp (hpWarning)
        if (hpWarning != null)
        {
            // Nếu máu <= 20, bật cảnh báo. Nếu > 20, tắt cảnh báo.
            bool shouldShowWarning = hp <= 20 && hp > 0;
            
            // Kiểm tra trạng thái hiện tại trước khi đặt
            if (hpWarning.activeSelf != shouldShowWarning)
            {
                hpWarning.SetActive(shouldShowWarning);
            }
        }

        // Cập nhật thanh máu UI
        hpFill.fillAmount = hp / maxHP;

        // Kiểm tra nếu chết (đã xử lý ở các bước trước)
        if (hp <= 0 && !hasPlayedDie)
        {
            hasPlayedDie = true;
            playerController.HandleDie();
        }
    }
}

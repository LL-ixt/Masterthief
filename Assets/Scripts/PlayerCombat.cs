using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public PlayerDamageController playerDamageController;
    private const float cost = -5f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && ScoreManager.Instance.CheckScore(-cost)) 
        {
            ScoreManager.Instance.UpdateScore(cost);
            HandleShooting();  
        }
        else if (PlayerController.Instance.readyToRefillHP && ScoreManager.Instance.CheckScore(5f))
        {
            playerDamageController.ReceiveDamage(-5f);
            ScoreManager.Instance.UpdateScore(-5f);
            PlayerController.Instance.readyToRefillHP = false;
        }
    }

    void HandleShooting()
    {
        // 1. Lấy vị trí chuột trên màn hình và chuyển sang tọa độ trong Game
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        
        // 2. So sánh vị trí chuột với vị trí Player (theo trục X)
        bool shootRight = worldMousePos.x > transform.position.x;

        // 3. Cập nhật tham số Animator
        animator.SetBool("isShootingRight", shootRight);
        animator.SetTrigger("isShooting");
        if (shootRight) animator.SetFloat("MoveX", 1);
        // 4. (Tùy chọn) Khởi tạo đạn ngay lập tức hoặc đợi Event trong Animation
        ShootBullet(worldMousePos);
    }

    void ShootBullet(Vector3 targetPos)
    {
        if (bulletPrefab == null) return;

        // Tính toán hướng bay từ Player đến điểm click
        Vector2 shootDirection = (targetPos - firePoint.position).normalized;

        // Tạo đạn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        // Gọi hàm Initialize giống như lính canh đã làm
        var bulletScript = bullet.GetComponent<BulletEnemyController>(); // Hoặc BulletPlayerController
        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
            bulletScript.SetShooter(gameObject);
        }
    }
}
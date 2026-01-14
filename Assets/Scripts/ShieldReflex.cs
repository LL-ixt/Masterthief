using UnityEngine;

public class ShieldReflex : MonoBehaviour
{
    private int count = 0;
    public PlayerCombat2 playerCombat2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu chạm vào đạn của lính
        if (other.CompareTag("Bullet"))
        {
            if (other.TryGetComponent<BulletEnemyController>(out var bulletScript))
            {
                // 1. Tính toán pháp tuyến (Normal) từ tâm khiên đến điểm va chạm
                Vector2 normal = (other.transform.position - transform.position).normalized;

                // 2. Lấy hướng bay hiện tại của đạn
                Vector2 incomingDir = bulletScript.GetDirection();

                // 3. Tính hướng phản xạ dựa trên bề mặt cong
                Vector2 reflectDir = Vector2.Reflect(incomingDir, normal);

                // 4. Cập nhật lại đạn: đổi hướng và đổi phe (để đạn bắn ngược lại lính)
                bulletScript.SetDirection(reflectDir);
                bulletScript.SetShooter(transform.parent.gameObject); 

                // Xoay Sprite đạn theo hướng mới
                float angle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                other.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                playerCombat2.isShielding = true;
                count++;
                if (count == 5)
                {
                    playerCombat2.isShielding = false;
                    gameObject.SetActive(false);
                    Debug.Log("Tắt khiên");
                    count = 0;
                } 
            }
        }
    }
}

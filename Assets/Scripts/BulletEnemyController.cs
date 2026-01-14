using UnityEngine;
using UnityEngine.Analytics;

public class BulletEnemyController : MonoBehaviour
{
    public float bulletSpeed = 1f; // Tốc độ đạn
    public float damageAmount = 10f; // Sát thương gây ra
    private GameObject shooter;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Tự hủy sau 3 giây để tránh lãng phí tài nguyên
        Destroy(gameObject, 3f);
    }

    public Vector2 GetDirection()
    {
        return rb.linearVelocity.normalized;
    }
    public void SetDirection(Vector2 direction)
    {
        // Áp dụng vận tốc ngay lập tức để đạn bay theo hướng được chỉ định
        rb.linearVelocity = direction.normalized * bulletSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // Cần có Rigidbody 2D trên Prefab đạn để dòng này hoạt động
    }

    public void SetShooter(GameObject newShooter)
    {
        shooter = newShooter;
    }
    // Sử dụng Trigger thay vì Collision cho đạn
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Nếu đạn trúng collision của nhân vật bắn, bỏ qua
        if (collision.gameObject == shooter) return;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đạn trúng người bắn, bỏ qua
        if (other.gameObject == shooter) return;

        // Kiểm tra sát thương
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.ReceiveDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
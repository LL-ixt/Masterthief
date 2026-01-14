using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float damageAmount = 10f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Cố gắng lấy Component implement IDamageable
        // (Player phải implement IDamageable)
        IDamageable target = other.GetComponent<IDamageable>();
        
        if (target != null)
        {
            // 2. Nếu tìm thấy (đó là Player), gọi hàm chung
            target.ReceiveDamage(damageAmount);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

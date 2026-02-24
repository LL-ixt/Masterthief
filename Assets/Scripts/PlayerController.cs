using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2; // Đã bỏ 'using System.Numerics;' không cần thiết

public class PlayerController : MonoBehaviour
{
    // Cấu hình vật lý và di chuyển
    public float moveSpeed = 1f; // Tăng tốc độ di chuyển lên một chút (tùy chọn)
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    private bool isDead = false;
    // Components
    Rigidbody2D rb;
    Animator animator; // Thêm Animator component

    // Biến cho di chuyển và va chạm
    Vector2 movementInput;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public static PlayerController Instance;
    public bool readyToCollect = false;
    public bool readyToRefillHP = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Lấy component Animator
    }

    private void FixedUpdate() {
        if (isDead) return;
        if (movementInput != Vector2.zero) {
            Debug.Log("movement input: " + movementInput);
            // 1. Thử di chuyển theo hướng input gốc
            bool success = TryMove(movementInput);

            if (!success) {
                // 2. Thử di chuyển theo trục X
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success) {
                    // 3. Thử di chuyển theo trục Y
                    TryMove(new Vector2(0, movementInput.y));
                }
            }
            
            // Cập nhật hướng di chuyển cho Animator (chỉ khi có input)
            UpdateAnimatorParameters(movementInput);
            
        } else {
            // Nếu không có input, chỉ cập nhật IsMoving = false
            animator.SetBool("IsMoving", false);
        }
    }

    // HÀM THÊM LOGIC ANIMATOR
    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction, // Phải truyền hướng mà chúng ta đang thử di chuyển
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );

        if (count == 0) {
            // Sử dụng "direction" đã chuẩn hóa (normalized) cho việc di chuyển 
            // để đảm bảo tốc độ ổn định khi di chuyển chéo.
            rb.MovePosition(rb.position + direction.normalized * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    // HÀM CẬP NHẬT CÁC THAM SỐ CHO BLEND TREE
    private void UpdateAnimatorParameters(Vector2 input)
    {
        // Cập nhật tham số IsMoving
        animator.SetBool("IsMoving", true);

        // Điều chỉnh hướng vector để giữ hướng cuối cùng mà nhân vật quay mặt tới
        if (input.x != 0 || input.y != 0)
        {
            if (input.x != 0) animator.SetFloat("MoveY", 0);
            else animator.SetFloat("MoveY", input.y);
            animator.SetFloat("MoveX", input.x);
        }
        
        // 
    }

    public void HandleDie()
    {
        if (isDead) return;
        isDead = true;

        // Ngừng mọi chuyển động vật lý
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; 

        // Chạy Animation chết
        animator.Play("player_die");

        // Gọi Coroutine để đợi animation xong rồi xử lý tiếp
        StartCoroutine(WaitAndFinishGame());
    }

    private System.Collections.IEnumerator WaitAndFinishGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (ScoreManager.Instance != null)
        {
            int currentIdx = ScoreManager.Instance.currentLevelIdx; // Lấy index level hiện tại
            
            // Lưu nội dung thông báo thua cuộc tùy theo level
            ScoreManager.Instance.SaveLevelResults(currentIdx, "Nhiệm vụ thất bại! Bạn đã bị bắt.");
        }
        // Chuyển sang cảnh thất bại
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultLose");
    }

    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    void OnCollect(InputValue value)
    {
        if (value.isPressed)
        {
            readyToCollect = true;
        }
    }

    void OnRefill(InputValue value)
    {
        if (value.isPressed)
        {
            readyToRefillHP = true;
        }
    }
}
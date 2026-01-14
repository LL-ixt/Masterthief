using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [Header("Cấu hình Khoảng cách")]
    public Transform target;
    public float chaseRange = 10f;       // Tầm phát hiện để đuổi
    public LayerMask obstacleMask;       // Layer tường/vật cản

    [Header("Cấu hình Hành động")]
    public float runningInterval = 1f;   // Chạy bao lâu thì dừng bắn
    public float shootingInterval = 1f;  // Thời gian chờ giữa mỗi lần bắn
    public GameObject bulletPrefab;

    public enum Action { Idle, Shoot, Run, Die };
    public Action currentAction = Action.Idle;

    [Header("Components")]
    private NavMeshAgent agent;
    private Animator animator;

    private float actionTimer = 0f;
    public float reward = 50f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Cấu hình NavMesh cho 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (target == null) return;
        // Xử lý các trạng thái
        float distance = Vector2.Distance(transform.position, target.position);
        if (currentAction == Action.Run && distance <= 0.3f) currentAction = Action.Idle;
        switch (currentAction)
        {
            case Action.Idle:
                HandleIdle();
                break;
            case Action.Run:
                HandleChasing();
                break;
            case Action.Shoot:
                HandleShooting();
                break;
            case Action.Die:
                HandleDie();
                break;
        }

        UpdateAnimator();
    }

    // --- XỬ LÝ LOGIC ---

    void HandleIdle()
    {
        agent.isStopped = true;
        
        // Nếu Player đi vào tầm mắt thì chuyển sang đuổi
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= chaseRange)
        {
            TransitionTo(Action.Run);
        }
    }

    void HandleChasing()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);

        actionTimer += Time.deltaTime;
        if (actionTimer >= runningInterval)
        {
            TransitionTo(Action.Shoot);
        }
    }

    void HandleShooting()
    {
        agent.isStopped = true;

        // Lấy thông tin trạng thái hiện tại của Animator tại Layer 0
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 1. Chỉ bắn một lần duy nhất khi vừa bắt đầu vào trạng thái Shoot
        if (actionTimer == 0f)
        {
            if (CanSeeTarget())
            {
                animator.SetBool("IsShooting", true);
                Shoot();
            }
        }

        actionTimer += Time.deltaTime;
        
        // 2. KIỂM TRA EXIT TIME BẰNG CODE:
        // Kiểm tra nếu đang ở trong clip "guard_shoot" 
        // và normalizedTime >= 1.0f (nghĩa là clip đã chạy xong 100%)
        if (stateInfo.IsName("guard_shoot") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("IsShooting", false);
            TransitionTo(Action.Idle);
        }
        else if (actionTimer >= shootingInterval) TransitionTo(Action.Idle);
    }

    void HandleDie()
    {
        // Dừng hẳn AI và va chạm để không cản đường Player
        
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false; 
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        } 
        
        // Tắt Collider nếu có để xác không chặn đạn/người
        

        // Lấy thông tin animation hiện tại
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Nếu chưa chuyển sang animation chết thì kích hoạt
        if (!stateInfo.IsName("guard_die"))
        {
            animator.Play("guard_die");
        }
        else
        {
            // Nếu đã diễn xong 100% clip chết (normalizedTime >= 1)
            if (stateInfo.normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
                ScoreManager.Instance.UpdateScore(reward);
            }
        }
    }

    void TransitionTo(Action nextAction)
    {
        currentAction = nextAction;
        actionTimer = 0f; // Reset timer mỗi khi đổi trạng thái
    }

    void Shoot()
    {
        animator.SetTrigger("IsShooting");

        Vector2 direction = (target.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        
        BulletEnemyController bulletController = bullet.GetComponent<BulletEnemyController>();
        if (bulletController != null)
        {
            bulletController.SetShooter(gameObject);
            bulletController.SetDirection(direction);
        }
    }

    bool CanSeeTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);
        
        // Raycast để xem có tường chắn giữa Guard và Player không
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);
        return hit.collider == null;
    }

    void UpdateAnimator()
    {
        // Kiểm tra an toàn: Agent phải đang bật và nằm trên NavMesh mới được lấy thông tin
        bool isMoving = false;
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            isMoving = !agent.isStopped && agent.velocity.sqrMagnitude > 0.01f;
        }
        
        animator.SetBool("IsMoving", isMoving);

        // Xác định hướng nhìn
        Vector2 lookDir;
        // Nếu đang di chuyển và agent hợp lệ thì lấy hướng vận tốc
        if (isMoving && agent.enabled && agent.isOnNavMesh)
        {
            lookDir = agent.velocity;
        }
        else
        {
            lookDir = target.position - transform.position; // Hướng về Player
        }

        // Lật mặt (Flip) dựa trên hướng nhìn
        if (Mathf.Abs(lookDir.x) > 0.15f)
        {
            bool isRight = lookDir.x > 0;
            //animator.SetBool("IsRight", isRight);
            transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
        }
    }

    void OnEnable()
{
    // Khi script được bật lên từ Signal
    if (agent == null) agent = GetComponent<NavMeshAgent>();
    
    agent.enabled = true; // Kích hoạt lại Agent
    if (agent.isOnNavMesh) agent.isStopped = false;
    
    // Đảm bảo Z không bị nhảy lung tung khi hiện lại
    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
}

    void OnDisable()
    {
        // Khi script bị tắt (lúc đang chạy Timeline)
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false; // Tắt Agent để nó không chiếm quyền Transform
        }
    }
}
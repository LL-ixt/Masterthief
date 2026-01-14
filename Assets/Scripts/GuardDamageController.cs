using UnityEngine;
using UnityEngine.UI;
public class GuardDamageController : MonoBehaviour, IDamageable
{
    private const float maxHP = 100f;
    public float hp = 100f;
    public float laserDamage = 10f;
    public Image hpFill;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveDamage(float damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0) {
            hp = 0;
            GuardController guardController = GetComponentInParent<GuardController>();
            if (guardController != null && guardController.currentAction != GuardController.Action.Die)
            {
                guardController.currentAction = GuardController.Action.Die;
            }
        }
        hpFill.fillAmount = hp/maxHP;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image hpBar;
    private const float maxHP = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHPBar(float currentHP)
    {
        float hpRatio = currentHP / maxHP;
        hpBar.fillAmount = hpRatio;
    }
}

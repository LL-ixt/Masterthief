using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class MainPainting1 : Interactable
{
    public Level1Manager levelManager;
    public ScoreManager energyManager;
    private const float bonusPoint = 100f;
    protected override void OnInteract()
    {
        CollectPainting();
        TriggerMainEvent();
    }

    void CollectPainting()
    {
        energyManager.UpdateScore(bonusPoint);
        // animation lấy tranh
        gameObject.SetActive(false);
    }

    protected virtual void TriggerMainEvent()
    {
        levelManager.TriggerCombatEvent();
    }
}

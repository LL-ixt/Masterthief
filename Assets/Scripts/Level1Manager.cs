using UnityEngine;
using UnityEngine.Playables;
public class Level1Manager : MonoBehaviour
{
    public PlayableDirector combatDirector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TriggerRobbingEvent()
    {
    
    }
    // public void TriggerSuccessEvent()
    // {
    //     director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    // }
    // public void TriggerAlarmEvent()
    // {
    //     director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    // }
    public void TriggerCombatEvent()
    {
        combatDirector.Play();
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class MainPainting3 : MainPainting1
{
    [Header("Cấu hình thoại Level 3")]
    public string[] characterLines = new string[] {
        "Haha, đúng như ta nghĩ, ngươi đã tìm đến bức tranh này.",
        "Tuy nhiên nó không phải hàng thật đâu!",
        "Bức tranh thật, ta đã giấu nó ở một nơi nào đó trên Trái Đất rồi...",
        "Ta phải làm thế... nếu các ngươi khai phá chúng, không biết điều gì sẽ xảy ra..."
    };

    public string characterName = "Chủ bảo tàng";
    public PlayableDirector director;
    public DialogueManager manager;
    public GameObject task;
    // Ghi đè lại hàm kích hoạt sự kiện
    protected override void TriggerMainEvent()
    {
        Debug.Log("Called");
        task.SetActive(false);
        levelManager.TriggerCombatEvent();
        manager.SetDirector(director);
        manager.StartDialogue(characterName, characterLines);
        //Invoke("StartTimelineAfterDialogue", 2f); 
    }

    void StartTimelineAfterDialogue()
    {
        // Gọi hàm của lớp cha (levelManager.TriggerCombatEvent)
        base.TriggerMainEvent(); 
    }
}
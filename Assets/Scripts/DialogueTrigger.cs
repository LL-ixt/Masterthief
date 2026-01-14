using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager manager;
    
    public string bossName = "Boss";
    [TextArea(3, 10)]
    public string[] bossLines = new string[] {
        "Xin chào, đặc vụ D.",
        "Như cậu đã biết, đặc vụ E vừa tìm ra tên của 3 bức tranh quan trọng...",
        "Từ thời xa xưa, các họa sĩ lừng danh đã phong ấn Quyền Năng Tối Thượng...",
        "Nhiệm vụ của cậu: Xâm nhập và lấy bức tranh thứ nhất."
    };

    public void TriggerStartDialogue() // Tạo hàm này để Signal gọi vào
    {
        // Gọi sang Manager và truyền dữ liệu có sẵn trong script này
        manager.StartDialogue(bossName, bossLines);
    }
}
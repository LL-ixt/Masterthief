using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    private int curButtonIdx = 0;
    private int maxSelectableButtons = 1; // Mặc định chỉ chọn được Level 1
    public GameObject[] shadow; // Mảng các đối tượng highlight/shadow cho các nút
    public GameObject[] lockIcons; // (Tùy chọn) Các icon ổ khóa để ẩn/hiện

    void Start()
    {
        UpdateUnlockedLevels();
        Debug.Log("max selectable " + maxSelectableButtons);
    }

    void UpdateUnlockedLevels()
    {
        if (DataManager.Instance != null)
        {
            int unlockedCount = 0;
            for (int i = 0; i < DataManager.Instance.gameData.levels.Count; i++)
            {
                bool isUnlocked = DataManager.Instance.gameData.levels[i].isUnlocked;
                
                // Cập nhật số lượng nút có thể tương tác
                if (isUnlocked) unlockedCount++;

                // Hiển thị hoặc ẩn ổ khóa dựa trên dữ liệu lưu
                if (lockIcons != null && i < lockIcons.Length)
                {
                    lockIcons[i].SetActive(!isUnlocked);
                }
                
                // (Tùy chọn) Làm mờ các nút chưa mở khóa
                // shadow[i].GetComponentInParent<Image>().color = isUnlocked ? Color.white : Color.gray;
            }
            maxSelectableButtons = unlockedCount;
            Debug.Log(maxSelectableButtons);
        }

        // Đảm bảo nút đầu tiên luôn được highlight
        for (int i = 0; i < shadow.Length; i++) shadow[i].SetActive(i == 0);
    }

    void Update()
    {
        // Di chuyển lên (hoặc sang trái)
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            MoveSelection(-1);
        }
        // Di chuyển xuống (hoặc sang phải)
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            MoveSelection(1);
        }
        // Chọn Level
        else if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            LoadSelectedLevel();
        }
    }

    void MoveSelection(int direction)
    {
        shadow[curButtonIdx].SetActive(false);
        
        curButtonIdx += direction;

        // Vòng lặp selection trong phạm vi các Level đã mở khóa
        if (curButtonIdx < 0) curButtonIdx = maxSelectableButtons - 1;
        else if (curButtonIdx >= maxSelectableButtons) curButtonIdx = 0;

        shadow[curButtonIdx].SetActive(true);
    }

    void LoadSelectedLevel()
    {
        // Tên Scene tương ứng với index: 0 -> Level1, 1 -> Level2, 2 -> Level3
        string levelName = "Level" + (curButtonIdx + 1);
        SceneManager.LoadScene(levelName);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public GameData gameData = new GameData();
    private string filePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Đường dẫn này hoạt động tốt trên cả PC và WebGL (IndexedDB)
            filePath = Path.Combine(Application.persistentDataPath, "game_save.json");
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm lưu dữ liệu
    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Đã lưu dữ liệu vào: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi khi lưu file: " + e.Message);
        }
    }

    // Hàm tải dữ liệu
    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                gameData = JsonUtility.FromJson<GameData>(json);
                Debug.Log("Đã tải dữ liệu thành công!");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Lỗi khi đọc file: " + e.Message);
                CreateDefaultData();
            }
        }
        else
        {
            Debug.Log("Không tìm thấy file lưu, khởi tạo dữ liệu mặc định.");
            CreateDefaultData();
        }
    }

    // Khởi tạo trạng thái ban đầu cho 3 Level
    private void CreateDefaultData()
    {
        gameData.levels.Clear();
        // Level 1 mặc định mở khóa
        gameData.levels.Add(new LevelData { levelName = "Level 1", isUnlocked = true, highscore = 0 });
        gameData.levels.Add(new LevelData { levelName = "Level 2", isUnlocked = false, highscore = 0 });
        gameData.levels.Add(new LevelData { levelName = "Level 3", isUnlocked = false, highscore = 0 });
        SaveGame();
    }

    // Hàm cập nhật điểm cao nhất cho một Level cụ thể
    public void UpdateHighScore(int levelIndex, float newScore)
    {
        // 1. Kiểm tra tính hợp lệ của chỉ số Level (index)
        if (levelIndex >= 0 && levelIndex < gameData.levels.Count)
        {
            // 2. Chỉ cập nhật nếu điểm mới cao hơn điểm đã lưu
            if (newScore > gameData.levels[levelIndex].highscore)
            {
                gameData.levels[levelIndex].highscore = newScore;
                Debug.Log($"Cập nhật Highscore mới cho {gameData.levels[levelIndex].levelName}: {newScore}");
                
                // 3. Lưu thay đổi xuống file JSON ngay lập tức
                SaveGame();
            }
        }
        else
        {
            Debug.LogWarning("Chỉ số Level không hợp lệ trong DataManager!");
        }
    }

    public void UnlockNextLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex > 2) return;
        gameData.levels[levelIndex].isUnlocked = true;
        SaveGame();
    }
}
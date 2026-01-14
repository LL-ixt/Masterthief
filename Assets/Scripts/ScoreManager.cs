using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI pointText;
    private float score = 100f;
    public Boolean hasCollectedSecretPainting = false;
    public string lastLogText;
    public float lastFinalScore;
    public int currentLevelIdx = 0;
    void Awake()
    {
        // Đây là bước quan trọng nhất:
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        UpdateCurrentLevelIdx();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentLevelIdx()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level1") currentLevelIdx = 0;
        else if (sceneName == "Level2") currentLevelIdx = 1;
        else if (sceneName == "Level3") currentLevelIdx = 2;
        else currentLevelIdx = -1; // Nếu ở Menu hoặc màn Result
        
        //Debug.Log("Đang ở màn chơi index: " + currentLevelIdx);
    }
    public Boolean CheckScore(float change)
    {
        if (score >= change) return true;
        return false; 
    }
    
    public void UpdateScore(float change)
    {
        score += change;
        pointText.text = "Score: " + score;
    }

    public void SaveLevelResults(int levelIndex, string log)
    {
        lastLogText = log;
        lastFinalScore = score;

        if (DataManager.Instance != null)
        {
            // Cập nhật Highscore cho level hiện tại
            DataManager.Instance.UpdateHighScore(levelIndex, score);
            // Mở khóa level tiếp theo
            DataManager.Instance.UnlockNextLevel(levelIndex + 1);
        }
    }
}

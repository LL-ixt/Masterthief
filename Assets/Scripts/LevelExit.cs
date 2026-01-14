using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải có để chuyển cảnh

public class LevelExit : MonoBehaviour
{
    public string winSceneName = "ResultWin";
    public string loseSceneName = "ResultLose";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            int levelIdx = 0; // Mặc định Level 1

            if (currentScene == "Level3")
            {
                levelIdx = 2;
                if (ScoreManager.Instance.hasCollectedSecretPainting)
                {
                    ScoreManager.Instance.SaveLevelResults(levelIdx, "Bạn đã lấy được bức tranh thứ ba!");
                    SceneManager.LoadScene(winSceneName);
                }
                else
                {
                    ScoreManager.Instance.SaveLevelResults(levelIdx, "Bạn chưa lấy được bức tranh thực sự!");
                    SceneManager.LoadScene(loseSceneName);
                }
            }
            else if (currentScene == "Level2")
            {
                levelIdx = 1;
                ScoreManager.Instance.SaveLevelResults(levelIdx, "Bạn đã lấy được bức tranh thứ hai!");
                SceneManager.LoadScene(winSceneName);
            }
            else // Level 1
            {
                levelIdx = 0;
                ScoreManager.Instance.SaveLevelResults(levelIdx, "Bạn đã lấy được bức tranh thứ nhất!");
                SceneManager.LoadScene(winSceneName);
            }
        }
    }
}
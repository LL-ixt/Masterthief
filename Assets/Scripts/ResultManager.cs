using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI logTextDisplay; // Kéo LogText vào đây
    public TextMeshProUGUI scoreTextDisplay; // Kéo ScoreText vào đây
    private int curButtonIdx = 0;
    private const int maxNumButtons = 3;
    public GameObject[] shadow;
    void Start()
    {
        logTextDisplay.text = ScoreManager.Instance.lastLogText;
        scoreTextDisplay.text = "Score: " + ScoreManager.Instance.lastFinalScore.ToString();
    }
    void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            shadow[curButtonIdx].SetActive(false);
            curButtonIdx--;
            if (curButtonIdx < 0) curButtonIdx = maxNumButtons-1;
            shadow[curButtonIdx].SetActive(true);
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            shadow[curButtonIdx].SetActive(false);
            curButtonIdx = (curButtonIdx + 1) % maxNumButtons;
            shadow[curButtonIdx].SetActive(true);
        }
        else if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            switch (curButtonIdx)
            {
                //RETRY
                case 0:
                    switch (ScoreManager.Instance.currentLevelIdx)
                    {
                        case 0:
                            SceneManager.LoadScene("Level1");
                            break;
                        case 1:
                            SceneManager.LoadScene("Level2");
                            break;
                        case 2:
                            SceneManager.LoadScene("Level3");
                            break;
                    }
                    break;
                //NEXT
                case 1:
                    if (ScoreManager.Instance.currentLevelIdx == 2) SceneManager.LoadScene("Ending");
                    else SceneManager.LoadScene("Select");
                    break;
                //MENU
                case 2:
                    SceneManager.LoadScene("Menu");
                    break;
            }
        }
    }
}

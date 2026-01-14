using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    private int curButtonIdx = 0;
    private const int maxNumButtons = 3;
    public GameObject[] shadow;
    void Start()
    {
        
    }

    // Update is called once per frame
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
                //PLAY
                case 0:
                    SceneManager.LoadScene("Select");
                    break;
                //EXIT:
                case 2:
                    Application.Quit();
                    break;
            }
        }
    }
}

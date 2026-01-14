using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public string firstCharacterName = "Boss";
    public string[] firstLines;
    public string secondCharacterName = "Endargon";
    public string[] secondLines;
    public string thirdCharacterName = "Agent D";
    public string[] thirdLines;
    public DialogueManager dialogueManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFirst()
    {
        dialogueManager.StartDialogue(firstCharacterName, firstLines);
    }

    public void PlaySecond()
    {
        dialogueManager.StartDialogue(secondCharacterName, secondLines);
    }

    public void PlayThird()
    {
        dialogueManager.StartDialogue(thirdCharacterName, thirdLines);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

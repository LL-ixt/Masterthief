using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject player;
    // Danh sách các câu thoại
    private Queue<string> sentences;
    private bool isTyping = false;
    private string currentSentence;
    public PlayableDirector director;

    void Awake()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        // Kiểm tra phím Enter hoặc phím Space
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (dialoguePanel.activeSelf)
            {
                DisplayNextSentence();
            }
        }
    }

    public void SetDirector(PlayableDirector newDirector)
    {
        director = newDirector;
    }
    public void StartDialogue(string characterName, string[] lines)
    {
        if (director != null && director.playableGraph.IsValid()) 
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        player.GetComponent<PlayerController>().enabled = false;
        dialoguePanel.SetActive(true);
        nameText.text = characterName;
        sentences.Clear();
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // Nếu đang gõ chữ mà nhấn Enter thì hiện hết câu luôn
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        // Nếu hết câu thoại thì đóng bảng
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f); // Tốc độ gõ chữ
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        if (director != null && director.playableGraph.IsValid()) 
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        dialoguePanel.SetActive(false);

        // Kích hoạt sự kiện tiếp theo ở đây (VD: bật script PlayerMovement)
        player.GetComponent<PlayerController>().enabled = true;
    }
}
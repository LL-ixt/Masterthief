using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;

public class SecretPainting : Collectables
{
    public Boolean hasCollected = false;
    [Header("Cấu hình thoại bí ẩn")]
    [TextArea(3, 10)]
    public string[] secretLines; // Các dòng thoại bí ẩn
    public string secretCharacterName = "Museum Owner";
    public PlayableDirector congrats;
    public ScoreManager energyManager;
    public DialogueManager dialogueManager;
    protected override void OnInteract()
    {
        // 1. Cộng điểm (kế thừa từ MainPainting1)
        // Nếu MainPainting1.OnInteract() có chứa logic Timeline, ta không dùng base.OnInteract()
        // Mà gọi lẻ các hàm cần thiết
        CollectSecret();
    }

    void CollectSecret()
    {
        // Cộng điểm thưởng lớn cho bảo vật bí ẩn
        if (energyManager != null)
        {
            energyManager.UpdateScore(200f); 
        }

        congrats.Play();
        ScoreManager.Instance.hasCollectedSecretPainting = true;
        Debug.Log("Đã tìm thấy bức tranh bí ẩn!");
        // Biến mất
    }

    public void ShowDialogue()
    {
        dialogueManager.SetDirector(congrats);
        dialogueManager.StartDialogue(secretCharacterName, secretLines);
        gameObject.SetActive(false);
    }
}
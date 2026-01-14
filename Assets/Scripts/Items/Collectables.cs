using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
public class Collectables : Interactable
{
    private float bonus = 10f;

    protected override void OnInteract()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.UpdateScore(bonus);
        }
        //PlayerStats.Instance.AddScore(score);
        Destroy(gameObject);
    }
}
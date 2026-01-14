using UnityEngine;
using UnityEngine.InputSystem;
public abstract class Interactable : MonoBehaviour
{
    protected bool playerInRange = false;

    private GameObject effectUI; // Square trắng

    protected virtual void Start()
    {
        effectUI = transform.Find("Effect").gameObject;
        effectUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (playerInRange && Keyboard.current.tKey.wasPressedThisFrame)
        {
            OnInteract();
        }
    }

    protected abstract void OnInteract();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            effectUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            effectUI.SetActive(false);
        }
    }
}

using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered by: " + collision.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger stay by: " + collision.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit by: " + collision.name);
    }
}

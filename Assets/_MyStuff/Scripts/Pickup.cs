using UnityEngine;

public class Pickup : MonoBehaviour
{
    int scoreValue = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision.gameObject);
        }
    }
    protected virtual void Collect(GameObject player)
    {
        GameManager.Instance.AddPoints(scoreValue);
        Destroy(gameObject);
    }
}

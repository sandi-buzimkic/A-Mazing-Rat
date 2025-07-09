using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public abstract float SpawnChance { get; }

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
        GameFlowController.Instance.ScoreManager.AddPoints(scoreValue);
        Destroy(gameObject);
    }
}

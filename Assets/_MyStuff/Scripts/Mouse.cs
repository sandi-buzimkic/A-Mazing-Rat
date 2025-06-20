using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private float moveSpeed = 50f;
    private bool isDragging = false;
    private bool isInvincible = false;
    SpriteRenderer spriteRenderer;
    private int maxLives = 3;
    private int currentLives;
    [SerializeField] private GameObject lives;
    [SerializeField] private GameObject lifePrefab;
    private void Start()
    {
        currentLives = maxLives;
        UpdateUI();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // Mouse or touch down
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.StartGame();
            isDragging = true;
        }

        // Mouse or touch up
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // While dragging
        if (isDragging)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 0f; // For 2D
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f; // Ensure it stays in 2D plane

            transform.position = Vector3.MoveTowards(transform.position, worldPos, moveSpeed * Time.deltaTime); ;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible)
        {
            return;
        }
        if (collision.CompareTag("Obstacle"))
        {
            TakeDamage();
        }
    }
    private void UpdateUI()
    {
        //Destroy all lives
        foreach (Transform child in lives.transform)
        {
            Destroy(child.gameObject);
        }
        //Make new lives
        for (int i = 0; i < currentLives; i++)
        {
            GameObject heart = Instantiate(lifePrefab);
            heart.transform.SetParent(lives.transform);
            heart.transform.localPosition = new Vector3(i*150f,0,0);
            heart.transform.localScale = Vector3.one;

        }
    }
    private void TakeDamage()
    {
        currentLives--;
        UpdateUI();
        if (currentLives == 0)
            Die();
        StartCoroutine(InvincibilityFrames());
    }
    private void Die()
    {
        GameManager.Instance.GameOver();
    }
    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class Mouse : MonoBehaviour
{
    private float moveSpeed = 200f;
    private bool isDragging = false;
    private bool isInvincible = false;
    SpriteRenderer spriteRenderer;
    private int maxLives = 3;
    private float heartSize = 2f;
    public int currentLives;
    [SerializeField] private GameObject lives;
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private UnityEngine.UI.Slider slider;

    private float tileSize = 1f;
    private int tilesWide = 7;

    private float targetTilt = 0f;
    private float tiltSpeed = 20f;
    public float RatAboveThumb = 0f;

    [SerializeField] private Transform proximityCircle;
    [SerializeField] private SpriteRenderer circleRenderer;
    [SerializeField] private float proximityRadius = 1.5f;
    float circleFadeDuration = 0.25f;
    float targetAlpha = 0.1f;
    Coroutine Fade;
    private void Start()
    {
        currentLives = maxLives;
        UpdateUI();
        LoadDistance();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = CharacterManager.Instance.RatSprite;
    }

    bool IsCloseToMouse(Vector3 target)
    {
        float dist = Vector3.Distance(transform.position, target);
        return dist <= proximityRadius;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            worldPos += new Vector3(0f, RatAboveThumb, 0f); //Add padding above thumb

            if (!IsCloseToMouse(worldPos))
                return;

            GameFlowController.Instance.StartGame();

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                StopCoroutine(Fade);

                circleRenderer.color = new Color(1f, 1f, 1f, 0f);
            }
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Fade = StartCoroutine(CircleFade());
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = -1f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = -1f;
            worldPos += new Vector3(0f, RatAboveThumb, 0f); //Add padding above thumb

            HandleDrag(worldPos);
        }
        else
        {
            targetTilt = Mathf.Lerp(targetTilt, 0f, Time.deltaTime * tiltSpeed);
        }
        Quaternion desiredRotation = Quaternion.Euler(0f, 0f, targetTilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * tiltSpeed);

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

    void OnTriggerStay2D(Collider2D collision)
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

    IEnumerator CircleFade()
    {
        if (circleRenderer == null) yield break;

        float t = 0f;
        while (t < circleFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(circleRenderer.color.a, targetAlpha, t / circleFadeDuration);
            Color c = circleRenderer.color;
            c.a = a;
            circleRenderer.color = c;
            yield return null;
        }

    }

    void HandleDrag(Vector3 worldPos)
    {
        float cameraCenterX = Camera.main.transform.position.x;
        float halfWidth = (tilesWide * tileSize) / 2f;
        worldPos.x = Mathf.Clamp(worldPos.x, cameraCenterX - halfWidth, cameraCenterX + halfWidth);

        Vector2 direction = transform.position - worldPos;

        if (direction.sqrMagnitude > 0.0001f)
        {
            targetTilt = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
        }
        else
        {
            targetTilt = Mathf.Lerp(targetTilt, 0f, Time.deltaTime * tiltSpeed);
        }

        transform.position = Vector3.MoveTowards(transform.position, worldPos, moveSpeed * Time.deltaTime);
        proximityCircle.position = transform.position - new Vector3(0f, RatAboveThumb, 0f);
    }
    public void UpdateUI()
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
            heart.transform.localPosition = new Vector3(0,- i * 50f, 0);
            heart.transform.localScale = Vector3.one * heartSize;
        }
    }

    private void UpdateProximityCircle()
    {
        if (proximityCircle == null) return;

        float diameter = proximityRadius * 2f;
        proximityCircle.localScale = new Vector3(diameter, diameter, 1f);
        proximityCircle.position = transform.position - new Vector3(0f, RatAboveThumb, 0f);
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
        GameFlowController.Instance.SetState(GameState.GameOver);
    }
    public void SetDistance()
    {
        RatAboveThumb = slider.value;
        PlayerPrefs.SetFloat("ratabovethumb", RatAboveThumb);
        UpdateProximityCircle();
    }
    public void LoadDistance()
    {
        RatAboveThumb = PlayerPrefs.GetFloat("ratabovethumb", 0f);
        slider.value = RatAboveThumb;
        UpdateProximityCircle();
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

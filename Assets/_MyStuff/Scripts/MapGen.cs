using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    public float scrollSpeed = 5f;
    [SerializeField] private List<GameObject> sections;
    [SerializeField] private GameObject pickup;
    float tilesWide = 8f;
    BoundsInt lastBounds;
    Vector3 leftEdge;
    float sectionTimer;
    float sectionInterval=0f;

    private float distanceTravelled = 0f;
    private float distancePerPoint = 1f;

    private float pickupSpawnChance = 0.2f;

    void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.5f, 10f));
    }
    private void GenerateSection()
    {
        int rand = Random.Range(0, sections.Count); 

        GameObject section = Instantiate(sections[rand],leftEdge, Quaternion.identity);
        
        section.transform.parent = gameObject.transform;

        Tilemap tilemap = section.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

        Vector3 offset = new Vector3(- tilesWide / 2f - bounds.x, -bounds.y * tilemap.cellSize.y, 0f);
        section.transform.position = leftEdge + offset;
        section.gameObject.name = "yuh";

        lastBounds = bounds;
        sectionInterval = (lastBounds.size.y * tilemap.cellSize.y) / (scrollSpeed / 1.5f);

        AttemptPickupSpawn(section);
    }
    private void DespawnOffscreenSections()
    {
        float bottomY = Camera.main.ViewportToWorldPoint(new Vector3(0f, -0.5f, 0f)).y;

        foreach (Transform child in transform)
        {
            Tilemap tm = child.GetComponentInChildren<Tilemap>();
            float tileHeight = (tm.cellBounds.y + tm.cellBounds.size.y) * tm.cellSize.y;

            if (child.position.y + tileHeight < bottomY)
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void AttemptPickupSpawn(GameObject section)
    {
        Transform spawnPoint = section.transform.Find("PickupSpawnPoint");

        if (spawnPoint == null) return;

        if (Random.value < pickupSpawnChance)
        {
            Debug.Log("spawned");
            //int rand = Random.Range(0, pickupPrefabs.Length);
            Instantiate(pickup, spawnPoint.position, Quaternion.identity, section.transform);
        }
        
    }

    private void Update()
    {
        if (GameManager.Instance.starting || GameManager.Instance.gameOver)
            return;

        float moveSpeed = scrollSpeed * Time.deltaTime;
        sectionTimer += Time.deltaTime;

        if (sectionTimer >= sectionInterval)
        {
            scrollSpeed += 0.1f;
            sectionTimer = 0f;
            GenerateSection();
        }

        distanceTravelled += moveSpeed;

        while (distanceTravelled >= distancePerPoint)
        {
            GameManager.Instance.AddPoints(1);
            distanceTravelled -= distancePerPoint;
        }
        transform.position += Vector3.down * moveSpeed;

        DespawnOffscreenSections();
    }
}

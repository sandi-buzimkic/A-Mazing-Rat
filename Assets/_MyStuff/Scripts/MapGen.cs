using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapGen : MonoBehaviour
{
    public float scrollSpeed = 5f;
    [SerializeField] private List<GameObject> sections;
    public List<GameObject> pickupEntries;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject floorPrefab;
    float tilesWide = 8f;
    BoundsInt lastBounds;
    Vector3 leftEdge;

    float sectionTimer;
    float sectionInterval=0f;

    float spawnHeight = 0f;
    float lastHeight = 0f;
    private float backgroundTileHeight = 30f;

    private float distanceTravelled = 0f;
    private float distancePerPoint = 1f;

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
        bool isFlipped = Random.value < 0.5f;
        if(isFlipped) { 
        FlipSection(section);
        }
        AttemptPickupSpawn(section, isFlipped);
        
    }
    private void FlipSection(GameObject section)
    {
        Transform location = section.transform;
        location.localScale = new Vector3(-1f,1f,1f);
        location.position = new Vector3(-location.position.x, location.position.y, location.position.z);
    }

    private void AttemptPickupSpawn(GameObject section, bool isFlipped)
    {
        Transform spawnPoint = section.transform.Find("PickupSpawnPoint");

        if (spawnPoint == null)
            return;

        GameObject selectedPickup = GetRandomPickup();
        if (selectedPickup == null)
            return;

        GameObject spawnedPickup = Instantiate(selectedPickup, spawnPoint.position, Quaternion.identity, section.transform);

        if (isFlipped)
        {
            Transform scale = spawnedPickup.transform;
            scale.localScale = new Vector3(-scale.localScale.x, scale.localScale.y, scale.localScale.z);
        }
    }

    private GameObject GetRandomPickup()
    {
        float rand = Random.value;

        var sortedEntries = pickupEntries.OrderBy(entry => entry.GetComponent<Pickup>().SpawnChance);


        foreach (var entry in sortedEntries)
        {
            if (rand <= entry.GetComponent<Pickup>().SpawnChance)
                return entry;
        }

        return null;
    }
    private void DespawnOffscreenSections()
    {
        float bottomY = Camera.main.ViewportToWorldPoint(new Vector3(0f, -0.5f, 0f)).y;

        foreach (Transform child in transform)
        {
            Tilemap tm = child.GetComponentInChildren<Tilemap>();

            if (tm == null) continue;
            
            float tileHeight = (tm.cellBounds.y + tm.cellBounds.size.y) * tm.cellSize.y;

            if (child.position.y + tileHeight < bottomY)
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void SpawnBackground()
    {
        spawnHeight += backgroundTileHeight;

        GameObject floor = Instantiate(floorPrefab, new Vector3(0f, spawnHeight, 10f), Quaternion.identity);

        lastHeight = floor.transform.position.y;

        floor.transform.SetParent(background.transform, false);

        float bottomY = Camera.main.ViewportToWorldPoint(new Vector3(0f, -0.5f, 0f)).y;


        foreach (Transform child in background.transform)
        {
            if (child.position.y < bottomY)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void Update()
    {
        if (GameFlowController.Instance.GameState != GameState.Playing)
            return;

        float moveSpeed = scrollSpeed * Time.deltaTime;
        sectionTimer += Time.deltaTime;

        if (sectionTimer >= sectionInterval)
        {
            scrollSpeed += 0.1f;
            sectionTimer = 0f;
            GenerateSection();
        }

        if (lastHeight < -background.transform.position.y + backgroundTileHeight)
        {
            SpawnBackground();
        }

        distanceTravelled += moveSpeed;

        while (distanceTravelled >= distancePerPoint)
        {
            GameFlowController.Instance.ScoreManager.AddPoints(1);
            distanceTravelled -= distancePerPoint;
        }
        transform.position += Vector3.down * moveSpeed;
        background.transform.position += Vector3.down * moveSpeed;
        DespawnOffscreenSections();
    }
}

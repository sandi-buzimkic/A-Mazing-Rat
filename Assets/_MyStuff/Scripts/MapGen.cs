using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;

public class MapGen : MonoBehaviour
{
    float scrollSpeed = 5f;
    [SerializeField] private List<GameObject> sections;
    float targetTilesWide = 8f;
    BoundsInt lastBounds;
    Vector3 leftEdge;
    float sectionTimer;
    float sectionInterval=100f; 
    void Start()
    {
        SetCameraSize();
        StartGenerating();
    }
    
    private void SetCameraSize()
    {
        float aspectRatio = (float)Screen.height / (float)Screen.width;
        Camera.main.orthographicSize = targetTilesWide * 0.5f * aspectRatio;
        leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.5f, 10f));
    }
    private void StartGenerating()
    {
        GameObject first = Instantiate(sections[0], leftEdge, Quaternion.identity);
        first.transform.parent = gameObject.transform;

        Tilemap tilemap = first.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

        Vector3 offset = new Vector3(-targetTilesWide / 2f - bounds.x, -bounds.y,0f);
        first.transform.position = leftEdge + offset;


        lastBounds = bounds;

        sectionInterval = (lastBounds.size.y * first.GetComponentInChildren<Tilemap>().cellSize.y) / (scrollSpeed/1.2f);
    }
    private void GenerateSection()
    {
        int rand = Random.Range(0, sections.Count); 

        GameObject section = Instantiate(sections[rand],leftEdge, Quaternion.identity);
        
        section.transform.parent = gameObject.transform;

        Tilemap tilemap = section.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

        Vector3 offset = new Vector3(-targetTilesWide / 2f - bounds.x, -bounds.y * tilemap.cellSize.y, 0f);
        section.transform.position = leftEdge + offset;
        section.gameObject.name = "yuh";
        
        BoundsInt currentBounds = section.GetComponent<Tilemap>().cellBounds;
        lastBounds = currentBounds;
        sectionInterval = (lastBounds.size.y * section.GetComponentInChildren<Tilemap>().cellSize.y) / (scrollSpeed / 1.5f);
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


    private void Update()
    {
        sectionTimer += Time.deltaTime;
        if (sectionTimer >= sectionInterval)
        {
            scrollSpeed += 0.1f;
            sectionTimer = 0f;
            GenerateSection();
        }

        transform.position -= new Vector3(0,scrollSpeed * Time.deltaTime,0);
        DespawnOffscreenSections();
    }
}

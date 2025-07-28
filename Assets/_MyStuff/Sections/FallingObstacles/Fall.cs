using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fall : MonoBehaviour
{
    public GameObject templateLight;
    public GameObject section;


    private CompositeCollider2D composite;
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer sprite;

    float fallTime = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerateLightsFromComposite());
        
    }

    IEnumerator GenerateLightsFromComposite()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        yield return new WaitForEndOfFrame();
        composite = GetComponent<CompositeCollider2D>();
        
        for (int i = 0; i < composite.shapeCount; i++)
        {
            section = Instantiate(templateLight.gameObject);
            section.transform.position = transform.position; 
            section.transform.SetParent(transform.parent, true);
            int pointCount = composite.GetPathPointCount(i);
            Vector2[] points = new Vector2[pointCount];
            composite.GetPath(i, points);

            Vector3[] usedPoints = new Vector3[pointCount];
            for (int j = 0; j < pointCount; j++)
            {
                usedPoints[j] = new Vector3(points[j].x, points[j].y, 0f);
            }

            Light2D shadow = section.GetComponent<Light2D>();
            shadow.SetShapePath(usedPoints);
        }
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        StartCoroutine(ManageSprite());
    }
    IEnumerator ManageSprite()
    {
        yield return new WaitForSeconds(3f);

        sprite.enabled = true;

       // float a = Mathf.Lerp(sprite.color.a, 255, t / circleFadeDuration);
        Color c = sprite.color;
        c.a = 0.2f;
        sprite.color = c;

        Vector3 endScale = transform.localScale;
        Vector3 startScale = endScale * 5f;
        transform.localScale = startScale;

        float elapsed = 0f;

        while (elapsed < fallTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fallTime);
            t *= t;
            float easedT = Mathf.Pow(t, 4);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            float a = Mathf.Lerp(sprite.color.a, 1f, easedT);
            c = sprite.color;
            c.a = a;
            sprite.color = c;

            yield return null;
        }

        Debug.Log(transform.localScale);
        polygonCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(section);
        Destroy(this.gameObject);
    }

}

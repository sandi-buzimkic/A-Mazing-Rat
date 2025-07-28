using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
public class CompositeToLightConverter : MonoBehaviour
{
    public GameObject templateLight;

    private CompositeCollider2D composite;
    void Start()
    {
        StartCoroutine(GenerateLightsFromComposite());
    }

    IEnumerator GenerateLightsFromComposite()
    {
        yield return new WaitForEndOfFrame();
        composite = GetComponent<CompositeCollider2D>();
        for (int i = 0; i < composite.shapeCount; i++) 
        { 
            GameObject section = Instantiate(templateLight.gameObject);
            section.transform.position = new Vector3(0, 0, 0);
            section.transform.SetParent(composite.transform, false);

            Light2D shadow = section.GetComponent<Light2D>();
            int pointCount = composite.GetPathPointCount(i);
            Vector2[] points = new Vector2[pointCount];
            composite.GetPath(i, points);

            Vector3[] usedPoints = new Vector3[pointCount];
            for (int j = 0; j < pointCount; j++)
            {
                usedPoints[j] = new Vector3(points[j].x, points[j].y, 0f);
            }

            // Apply the new shape
            shadow.SetShapePath(usedPoints);
        }
    }
}
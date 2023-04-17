using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    public float speed = 0.05f;

    private LineRenderer lineRenderer;

    [SerializeField] private GameObject linePrefab;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
    }

    /*void Update()
    {
        float offset = Time.time * speed;
        lineRenderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }*/

    public void DrawLine(Vector3 fromCoord, Vector3 toCoord)
    {
        lineRenderer.SetPosition(0, fromCoord);
        lineRenderer.SetPosition(1, toCoord);
    }
}

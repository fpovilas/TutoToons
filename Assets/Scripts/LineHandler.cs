using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    public float speed = 0.05f;

    private LineRenderer lineRenderer;

    [SerializeField] List<GameObject> Jewels;
    [SerializeField] private GameObject linePrefab;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        float offset = Time.time * speed;
        lineRenderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}

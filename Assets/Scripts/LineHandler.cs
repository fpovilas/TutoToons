using System.Collections;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;

    private LineRenderer lineRenderer;
    private bool isDone;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.25f;
    }

    public void DrawLine(Vector3 fromCoord, Vector3 toCoord)
    {
        lineRenderer.SetPosition(0, fromCoord);
        StartCoroutine(AnimateLine(fromCoord, toCoord));
        isDone = true;
    }

    public bool IsDone
    {
        set { isDone = value; }
        get { return isDone; }
    }

    private IEnumerator AnimateLine(Vector3 fromCoord, Vector3 toCoord)
    {
        float startTime = Time.time;

        Vector3 pos = fromCoord;
        while (pos != toCoord)
        {
            float t = (Time.time - startTime) / 1f;
            pos = Vector3.Lerp(fromCoord, toCoord, t);
            lineRenderer.SetPosition(1, pos);
            yield return null;
        }
    }
}
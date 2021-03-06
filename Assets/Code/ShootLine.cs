using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float timeToLive = 1f;
    public float startWidth = 0.05f;

    private float time;

    public void Init(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= timeToLive)
        {
            Destroy(gameObject);
        }
        else
        {
            lineRenderer.widthMultiplier = startWidth * (1f - (time / timeToLive));
        }
    }
}

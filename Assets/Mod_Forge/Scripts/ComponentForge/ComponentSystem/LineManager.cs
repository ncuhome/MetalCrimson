using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    private static LineManager instance;
    public static LineManager Insatnce { get { return instance; } }

    public LineRenderer lineRenderer;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawLine(Vector3 point1, Vector3 point2)
    {
        lineRenderer.useWorldSpace = true;
        float scaleX = Vector3.Magnitude(point1 - point2);
        lineRenderer.textureScale.Set(scaleX, 1);
        lineRenderer.SetPosition(0, point1);
        lineRenderer.SetPosition(1, point2);
    }
}

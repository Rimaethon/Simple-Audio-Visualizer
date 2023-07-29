using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public Transform startPoint; // The start point for lines
    public Transform endPoint; // The end point for lines
    public GameObject pointPrefab; // The prefab for the points
    public int numberOfPoints = 20; // The number of points to generate

    private GameObject[] points; // The generated points

    private void Awake()
    {
        // Initialize the points array
        points = new GameObject[numberOfPoints];
        

        for (int i = 0; i < numberOfPoints; i++)
        {
            // Calculate the progress along the line (from 0 to 1)
            float progress = (float)i / (numberOfPoints - 1);

            // Create a new point GameObject
            GameObject point = Instantiate(pointPrefab, Vector3.Lerp(startPoint.position, endPoint.position, progress), Quaternion.identity);

            // Add the new point to the points array
            points[i] = point;

            // If this is not the first point, add a LineRenderer to the previous point that draws a line to this point
            if (i > 0)
            {
                LineRenderer lineRenderer = points[i - 1].AddComponent<LineRenderer>();
                lineRenderer.useWorldSpace = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, points[i - 1].transform.position);
                lineRenderer.SetPosition(1, point.transform.position);
            }
        }
    }
}
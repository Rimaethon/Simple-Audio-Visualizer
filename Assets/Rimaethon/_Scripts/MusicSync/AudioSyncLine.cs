using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    public class AudioSyncLine : AudioSyncer
    {
        public Transform startPoint; // The start point for lines
        public Transform endPoint; // The end point for lines
        public float lineStartWidth = 0.1f; // Start width of lines
        public float lineEndWidth = 0.1f; // End width of lines
        public Material lineMaterial;

        private LineRenderer lineRenderer;

        protected override void Awake()
        {
            base.Awake();

            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineStartWidth;
            lineRenderer.endWidth = lineEndWidth;
            lineRenderer.positionCount = spectrumComponent.Levels.Length; // Number of positions equals number of frequency bands
            lineRenderer.useWorldSpace = true;
        }

        protected override void OnUpdate()
        {
            Vector3[] positions = new Vector3[spectrumComponent.Levels.Length];

            for (int i = 0; i < spectrumComponent.Levels.Length; i++)
            {
                float progress = (float)i / (spectrumComponent.Levels.Length - 1); // Progress from start to end point
                Vector3 pointPosition = Vector3.Lerp(startPoint.position, endPoint.position, progress); // Interpolate position along the line

                // Scale factor for the line's y-coordinate (height)
                float scaleY = spectrumComponent.Levels[i] * scaleFactor;
                Vector3 finalPosition = pointPosition + new Vector3(0, scaleY, 0); // Add the y offset to the position
                
                positions[i] = finalPosition;
                
                Debug.Log($"Setting position {i} to {finalPosition}");
            }

            lineRenderer.SetPositions(positions); // Set all positions on the line renderer
        }
    }
}
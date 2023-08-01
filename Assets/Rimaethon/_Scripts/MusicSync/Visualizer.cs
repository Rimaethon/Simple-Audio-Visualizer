using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    public class Visualizer : MonoBehaviour
    {
        private const float TargetScaleDescale = 0.1f;
        private const float Threshold = 0.06f;
        private float lerpSpeed = 20f;
        private float targetScale;
        [SerializeField] private float scaleMultiplier = 10f; // Adjust this value to control the scaling

        public void UpdateVisualizer(float amplitude)
        {
            targetScale = amplitude * scaleMultiplier; // Scale the target scale by a multiplier

            if (Mathf.Abs(transform.localScale.y - targetScale) < Threshold)
            {
                targetScale = TargetScaleDescale;
            }

            StopCoroutine(nameof(ScaleVisualizer));
            StartCoroutine(ScaleVisualizer());
        }

        private IEnumerator ScaleVisualizer()
        {
            Vector3 currentScale = transform.localScale;
            Vector3 targetScaleVector = new Vector3(1, targetScale, 1);

            float lerpTime = Mathf.Abs(targetScale - currentScale.y) / lerpSpeed;

            float timer = 0;
            while (timer < lerpTime)
            {
                currentScale.y = Mathf.Lerp(currentScale.y, targetScale, timer / lerpTime);
                transform.localScale = currentScale;
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScaleVector;
        }
    }
}
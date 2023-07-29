using System.Collections;
using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    public class AudioSyncScale : AudioSyncer
    {
        private Coroutine[] beatUpCoroutines;
        private Coroutine[] beatDownCoroutines;
        private float restScaleY = 0.1f;

        protected override void Awake()
        {
            base.Awake();
            beatUpCoroutines = new Coroutine[_childObjects.Length];
            beatDownCoroutines = new Coroutine[_childObjects.Length];
        }

        protected override void OnBeat(int barIndex)
        {
            Debug.Log($"AudioSyncScale: OnBeat called for bar {barIndex}");
            base.OnBeat(barIndex);

            Transform childToScale = _childObjects[barIndex];

            float scaleFactor = 1f;

           
            // Stop any ongoing scale up coroutine
            if (beatUpCoroutines[barIndex] != null)
                StopCoroutine(beatUpCoroutines[barIndex]);

            // Start scaling up
            beatUpCoroutines[barIndex] = StartCoroutine(MoveToScale(childToScale, scaleFactor, timeToBeat));

            // Stop any ongoing scale down coroutine
            if (beatDownCoroutines[barIndex] != null)
                StopCoroutine(beatDownCoroutines[barIndex]);

            // Start scaling down after a delay
            beatDownCoroutines[barIndex] = StartCoroutine(MoveToScale(childToScale, restScaleY, restSmoothTime));
        }


        private IEnumerator MoveToScale(Transform target, float targetY, float scaleTime)
        {
            Debug.Log($"AudioSyncScale: Starting MoveToScale for target {target.name} to Y {targetY} over {scaleTime} seconds");

            float initialY = target.localScale.y;
            float timeCounter = 0;

            while (timeCounter < scaleTime)
            {
                float newY = Mathf.Lerp(initialY, targetY, timeCounter / scaleTime);
                timeCounter += Time.deltaTime;
                target.localScale = new Vector3(1, newY, 1);

                Debug.Log($"AudioSyncScale: In MoveToScale for target {target.name}, timeCounter: {timeCounter}, newY: {newY}");

                yield return null;
            }

            target.localScale = new Vector3(1, targetY, 1);
            isBeat = false;

            Debug.Log($"AudioSyncScale: Finished MoveToScale for target {target.name}");
        }

    }
}

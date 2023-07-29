using System.Linq;
using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSpectrum))]
    public class AudioSyncer : MonoBehaviour 
    {
        protected Transform[] _childObjects;
        protected float timeToBeat = 0.4f;
        protected float restSmoothTime = 0.7f;
        protected bool isBeat;
        protected float baseScale = 3.0f;
        protected float scaleFactor = 1.0f;
        

        protected AudioSpectrum spectrumComponent;

        protected virtual void Awake()
        {
            _childObjects = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
        
            // Get the AudioSpectrum component
            spectrumComponent = GetComponent<AudioSpectrum>();
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        { 
            float[] spectrum = spectrumComponent.Levels;

            for (int i = 0; i < spectrum.Length; i++)
            {
                if (spectrum[i] > 0)
                {
                    OnBeat(i);
                }
            }

        }

        protected virtual void OnBeat(int barIndex)
        {
            float meanLevel = spectrumComponent.MeanLevel;
            float level = spectrumComponent.Levels[barIndex];

            if (meanLevel > 0)
            {
                scaleFactor = baseScale * (level / meanLevel);
            }

            //Transform child = _childObjects[barIndex];
            //child.localScale = Vector3.one * scaleFactor;
        }
    }

}
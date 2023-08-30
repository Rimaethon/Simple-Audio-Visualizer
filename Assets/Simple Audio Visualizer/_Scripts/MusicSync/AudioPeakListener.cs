using UnityEngine;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    public abstract class AudioPeakListener : MonoBehaviour
    {
        [SerializeField] protected int peakLevelToListen;
        protected AudioSpectrum AudioSpectrum;
        protected float Amplitude;

        protected virtual void Awake()
        {
            AudioSpectrum = GetComponentInParent<AudioSpectrum>();
        }

        protected virtual void Update()
        {
            Amplitude = AudioSpectrum.PeakLevels[peakLevelToListen];
            ProcessAmplitude(Amplitude);
        }

        protected abstract void ProcessAmplitude(float amplitude);
    }
}
using UnityEngine;
using System.Linq;

namespace Rimaethon._Scripts.MusicSync
{
    public class AudioSpectrum : MonoBehaviour
    {
        private const int NumberOfBands = 10;
        private const int TotalSampleSize = 1024;
        private const int SamplesToConsider = 198;
        private const float ScaleMin = 0.1f;
        private const float ScaleMax = 25f;

        private float[] _audioSpectrum;

        private float[] _frequencies = new float[]
        { 25.0f, 31.5f, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000,
            2500, 3150, 4000
        };
        [SerializeField] private Visualizer[] visualizers;

        private void Awake()
        {
            _audioSpectrum = new float[TotalSampleSize];
        }

        private void Update()
        {
            AudioListener.GetSpectrumData(_audioSpectrum, 0, FFTWindow.BlackmanHarris);
            ProcessBands();
            var i = Mathf.FloorToInt (25 / AudioSettings.outputSampleRate * 2.0f * _audioSpectrum.Length);
            Debug.Log(AudioSettings.outputSampleRate);
        }

        private void ProcessBands()
        {
            int remainingSamples = SamplesToConsider;
            int startSample = 0;

            for (int band = 0; band < NumberOfBands; band++)
            {
                int samplesPerBand = 3 + (band * 3);
                samplesPerBand = Mathf.Min(samplesPerBand, remainingSamples); // Ensure we don't exceed the remaining samples
                remainingSamples -= samplesPerBand;
                
                float averageAmplitude = _audioSpectrum.Skip(startSample).Take(samplesPerBand).Average();
                float scaledAmplitude = Mathf.Clamp(averageAmplitude * 100, ScaleMin, ScaleMax);

                visualizers[band].UpdateVisualizer(scaledAmplitude);

                startSample += samplesPerBand;
            }
        }
    }
}
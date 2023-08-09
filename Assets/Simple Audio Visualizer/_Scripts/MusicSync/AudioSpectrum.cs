using Simple_Audio_Visualizer._Scripts.Core.Enums;
using Simple_Audio_Visualizer._Scripts.Core.Interfaces;
using UnityEngine;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    public class AudioSpectrum : MonoBehaviour, IAudioSpectrumProvider
    {
        #region Fields

        private const int TotalSampleSize = 1024;
        private const float FallSpeed = 0.5f;
        private const float Sensibility = 15.0f;
        private float[] _levels;
        private float[] _rawSpectrumData;
        private float[] _frequencies;
        private const float Bandwidth = 2f;
        private float[] _peakLevels;
        private float[] _meanLevels;
        [SerializeField] private FrequencyPreset frequencyPreset = FrequencyPreset.Default;

        #endregion


        #region Monobehaviour functions

        private void Awake()
        {
            CheckBuffers();
        }


        private void Update()
        {
            CheckBuffers();

            AudioListener.GetSpectrumData(_rawSpectrumData, 0, FFTWindow.BlackmanHarris);

            FindFrequencyAmplitudes();
        }

        #endregion


        #region Private Functions

        public float GetPeakLevel(int index)
        {
            if (index >= 0 && index < _peakLevels.Length) return _peakLevels[index];
            return 0.1f;
        }

        public float GetMeanLevel(int index)
        {
            if (index >= 0 && index < _meanLevels.Length) return _meanLevels[index];
            return 0.1f;
        }

        public int GetPeakLevelsCount()
        {
            return _peakLevels.Length;
        }

        public int GetMeanLevelsCount()
        {
            return _meanLevels.Length;
        }

        private int FrequencyToSpectrumIndex(float frequency)
        {
            var floatingIndex =
                Mathf.FloorToInt(frequency / AudioSettings.outputSampleRate * 2.0f * _rawSpectrumData.Length);
            return Mathf.Clamp(floatingIndex, 0, _rawSpectrumData.Length - 1);
        }

        private void CheckBuffers()
        {
            switch (frequencyPreset)
            {
                case FrequencyPreset.Default:
                    _frequencies = new[] { 25f, 200f, 400f, 600f, 800f, 1200f, 1600f, 2400f, 3200f, 4400f };
                    break;
                case FrequencyPreset.Bass:
                    _frequencies = new[] { 20f, 40f, 60f, 80f, 100f, 120f, 140f, 160f };
                    break;
                case FrequencyPreset.Mid:
                    _frequencies = new[] { 200f, 500f, 800f, 1100f, 1400f, 1700f, 2000f };
                    break;
                case FrequencyPreset.High:
                    _frequencies = new[] { 2200f, 2600f, 3100f, 3600f, 4200f, 4800f, 5500f, 6200f, 7000f, 8000f };
                    break;
                case FrequencyPreset.FullRange:
                    _frequencies = new[] { 20f, 60f, 100f, 200f, 400f, 800f, 1600f, 3200f, 6400f, 12800f, 16000f };
                    break;
                case FrequencyPreset.Logarithmic:
                    _frequencies = new[] { 20f, 42f, 89f, 187f, 396f, 836f, 1764f, 3721f, 7847f, 16548f };
                    break;
            }

            if (_rawSpectrumData == null || _rawSpectrumData.Length != TotalSampleSize)
                _rawSpectrumData = new float[TotalSampleSize];
            var bandCount = _frequencies.Length;
            if (_levels == null || _levels.Length != bandCount)
            {
                _levels = new float[bandCount];
                _peakLevels = new float[bandCount];
                _meanLevels = new float[bandCount];
            }
        }

        private void FindFrequencyAmplitudes()
        {
            var fallDown = FallSpeed * Time.deltaTime;
            var filter = Mathf.Exp(-Sensibility * Time.deltaTime);

            for (var i = 0; i < _levels.Length; i++)
            {
                var iMin = FrequencyToSpectrumIndex(_frequencies[i] / Bandwidth);
                var iMax = FrequencyToSpectrumIndex(_frequencies[i] * Bandwidth);

                var bandMax = 0.0f;
                for (var j = iMin; j <= iMax; j++) bandMax = Mathf.Max(bandMax, _rawSpectrumData[j]);

                _levels[i] = bandMax;
                _peakLevels[i] = Mathf.Max(_peakLevels[i] - fallDown, bandMax);
                _meanLevels[i] = bandMax - (bandMax - _meanLevels[i]) * filter;
            }
        }

        #endregion
    }
}
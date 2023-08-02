using System;
using UnityEngine;


namespace Rimaethon._Scripts.MusicSync
{
    public class AudioSpectrum : MonoBehaviour
    {
        #region Fields

        private const int TotalSampleSize = 1024;
        private const float FallSpeed = 0.5f;
        private const float Sensibility = 15.0f;
        private float[] _levels;
        private float[] _rawSpectrumData;
        public float[] _frequencies = new float[] {};
        private const float  Bandwidth=2f;
        private float[] _peakLevels;
        private float[] _meanLevels;
        
        #endregion

        #region Properties
        
        public float[] Levels => _levels;
        public float[] PeakLevels => _peakLevels;
        public float[] MeanLevels => _meanLevels;

        #endregion
        
       


        
        #region Monobehaviour functions
       
        
        private void Start()
        {
            CheckBuffers ();

        }

        void Update ()
        {
            CheckBuffers ();

            AudioListener.GetSpectrumData (_rawSpectrumData, 0, FFTWindow.BlackmanHarris);
            
            FindFrequencyAmplitudes ();

        }
        #endregion
        
        
        
        #region Private Functions
        int FrequencyToSpectrumIndex (float frequency)
        {
            var floatingIndex = Mathf.FloorToInt (frequency / AudioSettings.outputSampleRate * 2.0f * _rawSpectrumData.Length);
            return Mathf.Clamp (floatingIndex, 0, _rawSpectrumData.Length - 1);
        }
        
        void CheckBuffers ()
        {
            if (_rawSpectrumData == null || _rawSpectrumData.Length != TotalSampleSize) {
                _rawSpectrumData = new float[TotalSampleSize];
            }
            var bandCount = _frequencies.Length;
            if (_levels == null || _levels.Length != bandCount) {
                _levels = new float[bandCount];
                _peakLevels = new float[bandCount];
                _meanLevels = new float[bandCount];
            }
        }
        
        void FindFrequencyAmplitudes ()
        {
            
            float fallDown = FallSpeed * Time.deltaTime;
            float filter = Mathf.Exp (-Sensibility * Time.deltaTime);
            
            for (int i = 0; i < _levels.Length; i++) {
                int iMin = FrequencyToSpectrumIndex (_frequencies [i] / Bandwidth);
                int iMax = FrequencyToSpectrumIndex (_frequencies [i] * Bandwidth);

                var bandMax = 0.0f;
                for (int j = iMin; j <= iMax; j++) {
                    bandMax = Mathf.Max (bandMax, _rawSpectrumData [j]);
                }

                _levels [i] = bandMax;
                _peakLevels [i] = Mathf.Max (_peakLevels [i] - fallDown, bandMax);
                _meanLevels [i] = bandMax - (bandMax - _meanLevels [i]) * filter;
            }
        }

        #endregion

    }
}
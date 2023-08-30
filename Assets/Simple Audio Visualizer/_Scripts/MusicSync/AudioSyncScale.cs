using UnityEngine;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    public class AudioSyncScale : AudioPeakListener
    {
        private const float TargetScaleDescale = 0.1f;
        private const float Threshold = 0.06f;
        private const float LerpTime = 0.1f;
        [SerializeField] private float scaleMultiplier = 10f;

        private float _targetScale;
        private float _currentScale;
        private float _velocity;

        #region Overrided Functions

        protected override void Awake()
        {
            base.Awake();
            _currentScale = transform.localScale.y;
        }

        protected override void ProcessAmplitude(float amplitude)
        {
            _targetScale = amplitude * scaleMultiplier;
            ScaleObjectsSmoothly();
        }


        #endregion
    
        #region Custom Functions
        
        
        private void ScaleObjectsSmoothly()
        {
            if (Mathf.Abs(_currentScale - _targetScale) < Threshold)
            {
                _targetScale = TargetScaleDescale;
            }

            _currentScale = Mathf.SmoothDamp(_currentScale, _targetScale, ref _velocity, LerpTime);

            Vector3 newScale = new Vector3(1, _currentScale, 1);
            transform.localScale = newScale;
        }
        

        #endregion
    }
}
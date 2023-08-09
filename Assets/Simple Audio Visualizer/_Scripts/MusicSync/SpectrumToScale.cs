using System;
using UnityEngine;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    public class SpectrumToScale : SpectrumBehaviour
    {
        
        
        #region Fields

        private const float MinScale = 0.1f;
        private const float MaxScale = 25f;
        private float _newYScale;

        #endregion
        #region Private Functions

        protected override void OnBeat(int beatIndex)
        {
            var currentScale = ChildObjects[beatIndex].localScale;
            if (Math.Abs(MaxScale - currentScale.y) < 0.1f)
            {
                var newYScale = Mathf.Lerp(currentScale.y, currentScale.y - 2f, Time.deltaTime * 10f);
            }

            var targetYScale = Mathf.Clamp(AudioSpectrumProvider.GetPeakLevel(beatIndex) * 100, MinScale, MaxScale);
            _newYScale = Mathf.Lerp(currentScale.y, targetYScale, Time.deltaTime * 10f);
            ChildObjects[beatIndex].localScale = new Vector3(1, _newYScale, 1);
        }

        #endregion

    }
}
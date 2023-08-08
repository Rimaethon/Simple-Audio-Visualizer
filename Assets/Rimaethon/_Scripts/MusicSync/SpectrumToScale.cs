using System;
using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    public class SpectrumToScale : SpectrumBehaviour
    {
        #region Fields
        
        private const float MinScale = 0.1f;
        private const float MaxScale = 25f;
        private const float BeatScaleIncrement = 3f;
        private float _newYScale;
        

        #endregion
        
        
        #region Private Functions 
        
        protected override void OnBeat(int beatIndex)
        {
            Vector3 currentScale= ChildObjects[beatIndex].localScale;
            if (Math.Abs( MaxScale-currentScale.y ) < 0.1f)
            {
                float newYScale = Mathf.Lerp(currentScale.y, currentScale.y-2f, Time.deltaTime * 10f);
            }
            float targetYScale = Mathf.Clamp(AudioSpectrumProvider.GetPeakLevel(beatIndex)*100, MinScale, MaxScale);
            _newYScale = Mathf.Lerp(currentScale.y, targetYScale, Time.deltaTime * 10f);
            ChildObjects[beatIndex].localScale = new Vector3(1, _newYScale, 1);
        }
        
       #endregion
       
        

        
    }

       
    
}
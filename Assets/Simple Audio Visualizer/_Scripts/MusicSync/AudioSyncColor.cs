using UnityEngine;
using UnityEngine.UI;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    [RequireComponent(typeof(Image))]
    public class AudioSyncColor : AudioPeakListener
    {
        private Image _imageComponent;

        #region Overrided Functions

        protected override void Awake()
        {
            base.Awake();
            _imageComponent = GetComponent<Image>();
        }

        protected override void ProcessAmplitude(float amplitude)
        {
            Color targetColor = Color.Lerp(Color.yellow, Color.red, amplitude);
            _imageComponent.color = targetColor;
        }
        
        #endregion
   
    }
}
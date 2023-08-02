using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    public class Visualizer : MonoBehaviour
    {
        private AudioSpectrum _audioSpectrum;
        [SerializeField] private float frequencyToListen;

        private void Awake()
        {
            _audioSpectrum = GetComponentInParent<AudioSpectrum>();
            _audioSpectrum._frequencies = new float[] {frequencyToListen};
            
        }
        
        
        private void Update()
        {
            
                var scale = _audioSpectrum.PeakLevels[0]*100 ;
                transform.localScale = new Vector3(1, scale, 1);
            
        }


        
        
    }
}
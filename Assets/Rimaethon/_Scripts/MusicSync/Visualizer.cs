using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{
    public class Visualizer : MonoBehaviour
    {
        private AudioSpectrum _audioSpectrum;
        private float scaleHolder;

        private void Awake()
        {
            _audioSpectrum = GetComponentInParent<AudioSpectrum>();
        }

        private void Update()
        {
            for (int i = 0; i < _audioSpectrum.PeakLevels.Length; i++)
            {
                float scale = _audioSpectrum.PeakLevels[i] * 100;
                scaleHolder = Mathf.Lerp(transform.GetChild(i).localScale.y, scale, Time.deltaTime * 10f);
                transform.GetChild(i).localScale = new Vector3(1, scaleHolder, 1);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Rimaethon._Scripts.MusicSync
{
    public interface IAudioSpectrumProvider
    {
        float GetPeakLevel(int index);
    }

    [RequireComponent(typeof(Image))]
    public class AudioSyncColor : MonoBehaviour
    {
        private IAudioSpectrumProvider _audioSpectrumProvider;
        [SerializeField] private float frequencyToListen;
        private float normalizedPeakLevel;

        // Dependency Injection through the Awake method
        private void Awake()
        {
            _audioSpectrumProvider = GetComponentInParent<IAudioSpectrumProvider>();
        }

        private void ChangeColors()
        {
            normalizedPeakLevel = _audioSpectrumProvider.GetPeakLevel(0) / 256f;
            var color = new Color(normalizedPeakLevel, normalizedPeakLevel, normalizedPeakLevel);
            GetComponent<Image>().color = color;
        }
    }
}
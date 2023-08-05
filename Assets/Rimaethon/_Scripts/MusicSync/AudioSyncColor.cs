using UnityEngine;
using UnityEngine.UI;

namespace Rimaethon._Scripts.MusicSync
{
    public enum Colors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Magenta,
        Cyan
    }

    [RequireComponent(typeof(Image))]
    public class AudioSyncColor : MonoBehaviour
    {
        private IAudioSpectrumProvider _audioSpectrumProvider;
        private Image _image;
        [SerializeField] private int frequencyBandIndex = 0; 

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Awake()
        {
            _audioSpectrumProvider = GetComponentInParent<IAudioSpectrumProvider>();
        }

        private void Update()
        {
            ChangeColors();
           
        }

        private void ChangeColors()
        {
            float meanLevel = _audioSpectrumProvider.GetMeanLevel(frequencyBandIndex);
            float peakLevel = _audioSpectrumProvider.GetPeakLevel(frequencyBandIndex);

            // Avoid division by zero
            if (peakLevel == 0)
                return;

            float ratio = meanLevel / peakLevel;

            // Determine the color based on the ratio
            Colors colorEnum = GetColorByRatio(ratio);

            // Convert the enum to a Unity color
            Color color = ConvertEnumToColor(colorEnum);

            _image.color = color;
        }

        private Colors GetColorByRatio(float ratio)
        {
            int index = Mathf.FloorToInt(ratio * 6);

            index = Mathf.Clamp(index, 0, 5);

            return (Colors)index;
        }

        
        private Color ConvertEnumToColor(Colors colorEnum)
        {
            switch (colorEnum)
            {
                case Colors.Red: return Color.red;
                case Colors.Green: return Color.green;
                case Colors.Blue: return Color.blue;
                case Colors.Yellow: return Color.yellow;
                case Colors.Magenta: return Color.magenta;
                case Colors.Cyan: return Color.cyan;
                default: return Color.white; 
            }
        }
    }
}

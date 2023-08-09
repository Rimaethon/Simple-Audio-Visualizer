using UnityEngine;
using UnityEngine.UI;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    public class SpectrumToColor : SpectrumBehaviour
    {
        #region Fields

        private int _currentColorIndex;

        #endregion


        #region Private Functions

        private Color ConvertIndexToColor(int colorIndex)
        {
            switch (colorIndex)
            {
                case 1: return Color.red;
                case 2: return Color.green;
                case 3: return Color.blue;
                case 4: return Color.yellow;
                case 5: return Color.magenta;
                case 6: return Color.cyan;
                default: return Color.white;
            }
        }

        #endregion


        #region Protected Functions

        protected override void OnBeat(int beatIndex)
        {
            var childImage = ChildObjects[beatIndex].GetComponent<Image>();
            _currentColorIndex = Random.Range(1, 7);
            childImage.color = ConvertIndexToColor(_currentColorIndex);
        }

        #endregion
    }
}
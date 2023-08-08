using Rimaethon._Scripts.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Rimaethon._Scripts.MusicSync
{
    public class SpectrumToColor : SpectrumBehaviour
    {
        #region Fields

        private int _currentColorIndex = 0;


        #endregion


        #region Private Functions

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

        #endregion


        #region Protected Functions
        protected override void OnBeat(int beatIndex)
        {
            Image childImage=ChildObjects[beatIndex].GetComponent<Image>();
            _currentColorIndex = (_currentColorIndex + 1) % System.Enum.GetValues(typeof(Colors)).Length;
            
        }


        #endregion
       
       
    }
}
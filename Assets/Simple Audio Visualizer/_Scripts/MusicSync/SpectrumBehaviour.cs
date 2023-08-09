using Simple_Audio_Visualizer._Scripts.Core.Interfaces;
using UnityEngine;

namespace Simple_Audio_Visualizer._Scripts.MusicSync
{
    public abstract class SpectrumBehaviour : MonoBehaviour
    {
        #region Protected Functions

        protected abstract void OnBeat(int beatIndex);

        #endregion

        #region Fields

        protected IAudioSpectrumProvider AudioSpectrumProvider;
        protected Transform[] ChildObjects;
        private float _beatCooldown;

        #endregion


        #region MonoBehaviour Functions

        protected virtual void Awake()
        {
            AudioSpectrumProvider = GetComponentInParent<IAudioSpectrumProvider>();
            GetChildObjects();

            if (AudioSpectrumProvider == null) Debug.LogError("No IAudioSpectrumProvider component found on parent.");
        }

        protected virtual void Update()
        {
            BeatChecker();
        }

        #endregion


        #region Private Functions

        private void BeatChecker()
        {
            for (var i = 0; i < AudioSpectrumProvider.GetPeakLevelsCount(); i++)
                if (AudioSpectrumProvider.GetMeanLevel(i) < AudioSpectrumProvider.GetPeakLevel(i))
                    OnBeat(i);
        }

        private void GetChildObjects()
        {
            ChildObjects = new Transform[gameObject.transform.childCount];

            for (var i = 0; i < gameObject.transform.childCount; i++)
                ChildObjects[i] = gameObject.transform.GetChild(i);
        }

        #endregion
    }
}
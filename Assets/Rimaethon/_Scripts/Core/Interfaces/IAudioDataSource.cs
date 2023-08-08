using UnityEngine;

namespace Rimaethon._Scripts.Core.Enums
{
    public interface IAudioDataSource
    {
        void GetSpectrumData(float[] audioData, int channel, FFTWindow windowType);
    }
}
using UnityEngine;

namespace Simple_Audio_Visualizer._Scripts.Core.Interfaces
{
    public interface IAudioDataSource
    {
        void GetSpectrumData(float[] audioData, int channel, FFTWindow windowType);
    }
}
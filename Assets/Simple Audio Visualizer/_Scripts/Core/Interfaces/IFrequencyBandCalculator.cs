using Simple_Audio_Visualizer._Scripts.Core.Enums;

namespace Simple_Audio_Visualizer._Scripts.Core.Interfaces
{
    public interface IFrequencyBandCalculator
    {
        void Initialize(FrequencyPreset frequencyPreset);
        void FindFrequencyAmplitudes(float[] spectrumData);
        float GetPeakLevel(int index);
        float GetMeanLevel(int index);
        int GetPeakLevelsCount();
        int GetMeanLevelsCount();
    }
}
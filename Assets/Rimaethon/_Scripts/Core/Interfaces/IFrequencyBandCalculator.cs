namespace Rimaethon._Scripts.Core.Enums
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
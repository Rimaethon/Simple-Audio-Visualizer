namespace Simple_Audio_Visualizer._Scripts.Core.Interfaces
{
    public interface IAudioSpectrumProvider
    {
        float GetPeakLevel(int index);
        float GetMeanLevel(int index);
        int GetPeakLevelsCount();
        int GetMeanLevelsCount();
    }
}
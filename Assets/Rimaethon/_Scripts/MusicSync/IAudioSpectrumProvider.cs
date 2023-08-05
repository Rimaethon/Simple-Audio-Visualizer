namespace Rimaethon._Scripts.MusicSync
{
    public interface IAudioSpectrumProvider
    {
        float GetPeakLevel(int index);
        float GetMeanLevel(int index);
    }
}
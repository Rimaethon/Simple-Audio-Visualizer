namespace Rimaethon._Scripts.MusicSync
{
    public partial interface IAudioSpectrumProvider
    {
        float GetPeakLevel(int index);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    #region Public variables
    public float fallSpeed = 0.08f;
    public float sensibility = 8.0f;
    #endregion

    #region Private variables
    private float[] rawSpectrum;
    public float[] levels;
    private float[] peakLevels;
    private float[] meanLevels;
    private float meanLevel;

    // Set band type
    private float[] middleFrequencies = new float[] {
        100, 400, 700, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500,
        9000, 9500, 10000, 10500, 11000
    };

    private float bandwidth = 1.414f;
    private float levelScaler = 1f;
    #endregion

    #region Public property
    public float[] Levels => levels;

    public float[] PeakLevels => peakLevels;

    public float MeanLevel => meanLevel;
    #endregion

    void Update()
    {
        CheckBuffers();
        GetSpectrumData();
        CalculateLevels();
        SaveSpectrumDataToFile();
    }

    void SaveSpectrumDataToFile()
    {
        // Create a list to store frequency-amplitude pairs
        var frequencyAmplitudePairs = new List<KeyValuePair<float, float>>();

        // Populate the list with frequencies and their amplitudes
        for (int i = 0; i < rawSpectrum.Length; i++)
        {
            float frequency = i * AudioSettings.outputSampleRate / 2.0f / rawSpectrum.Length;
            if (frequency <= 20000)  // Only consider frequencies up to 20000 Hz
            {
                frequencyAmplitudePairs.Add(new KeyValuePair<float, float>(frequency, rawSpectrum[i]));
            }
        }

        // Sort the list in descending order of amplitude
        frequencyAmplitudePairs.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // Write the sorted list to the file
        using (var writer = new System.IO.StreamWriter("C:/Unity Projects/Pace-Maker/Assets/Rimaethon/_Scripts/UI/spectrum.txt"))
        {
            foreach (var pair in frequencyAmplitudePairs)
            {
                writer.WriteLine($"{pair.Key} Hz: {pair.Value}");
            }
        }
    }

    void GetSpectrumData()
    {
        AudioListener.GetSpectrumData(rawSpectrum, 0, FFTWindow.BlackmanHarris);
    }

    void CalculateLevels()
    {
        var falldown = fallSpeed * Time.deltaTime;
        var filter = Mathf.Exp(-sensibility * Time.deltaTime);
        float totalLevel = 0;

        for (var bandIndex = 0; bandIndex < levels.Length; bandIndex++)
        {
            int minFrequencyIndex = FrequencyToSpectrumIndex(middleFrequencies[bandIndex] / bandwidth);
            int maxFrequencyIndex = FrequencyToSpectrumIndex(middleFrequencies[bandIndex] * bandwidth);

            var bandMax = CalculateBandMax(minFrequencyIndex, maxFrequencyIndex);
            levels[bandIndex] = bandMax * levelScaler;
            peakLevels[bandIndex] = Mathf.Max(peakLevels[bandIndex] - falldown, bandMax)*levelScaler;
            meanLevels[bandIndex] = bandMax - (bandMax - meanLevels[bandIndex]) * filter*levelScaler;

            totalLevel += levels[bandIndex];
        }

        meanLevel = totalLevel / levels.Length;
    }
    
    void CheckBuffers()
    {
        int numberOfSamples = 1024;  // Set number of samples

        if (rawSpectrum == null || rawSpectrum.Length != numberOfSamples)
        {
            rawSpectrum = new float[numberOfSamples];
        }

        if (levels == null || levels.Length != middleFrequencies.Length)
        {
            levels = new float[middleFrequencies.Length];
            peakLevels = new float[middleFrequencies.Length];
            meanLevels = new float[middleFrequencies.Length];
        }
    }
    int FrequencyToSpectrumIndex (float frequency)
    {
        var index = Mathf.FloorToInt(frequency / AudioSettings.outputSampleRate * 2.0f * rawSpectrum.Length);
        return Mathf.Clamp(index, 0, rawSpectrum.Length - 1);
    }
    float CalculateBandMax(int minFrequencyIndex, int maxFrequencyIndex)
    {
        var bandMax = 0.0f;
        for (var frequencyIndex = minFrequencyIndex; frequencyIndex <= maxFrequencyIndex; frequencyIndex++)
        {
            bandMax = Mathf.Max(bandMax, rawSpectrum[frequencyIndex]);
        }

        return bandMax;
    }
}

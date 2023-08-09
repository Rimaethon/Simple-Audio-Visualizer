using System.Collections;
using UnityEngine;

[System.Serializable]
public class AmplitudeSlice
{
    public float timestamp; // Timestamp in milliseconds
    public float[] amplitudes; // Top 10 amplitude values
}

[System.Serializable]
public class AmplitudeDataContainer
{
    public AmplitudeSlice[] amplitudeData;
}

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the audio source playing the song
    public TextAsset analysisDataJson; // JSON file containing the analysis results
    public Transform parentObject; // Parent object containing the 10 child objects to scale

    private AmplitudeSlice[] amplitudeData; // Array of amplitude slices loaded from JSON

    private void Start()
    {
        LoadAnalysisData(); // Load the amplitude data from the JSON
        StartCoroutine(SynchronizeWithAudio()); // Start synchronization coroutine
        audioSource.Play(); // Play the audio
    }

    private void LoadAnalysisData()
    {
        // Deserialize the JSON analysis data into an instance of AmplitudeDataContainer
        AmplitudeDataContainer container = JsonUtility.FromJson<AmplitudeDataContainer>(analysisDataJson.text);
        amplitudeData = container.amplitudeData; // Extract the array from the container
    }


    private IEnumerator SynchronizeWithAudio()
    {
        int index = 0;
        while (audioSource.isPlaying)
        {
            // Get the current playback time in milliseconds
            float currentPlaybackTime = audioSource.time * 1000f;

            // Check if the current playback time corresponds to the next amplitude slice
            if (index < amplitudeData.Length && currentPlaybackTime >= amplitudeData[index].timestamp)
            {
                for (int i = 0; i < 10 && i < parentObject.childCount; i++)
                {
                    Transform child = parentObject.GetChild(i);
                    float targetScale = amplitudeData[index].amplitudes[i];
                    StartCoroutine(ScaleObject(child, targetScale));
                }
                index++;
            }

            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator ScaleObject(Transform obj, float targetScale)
    {
        float startTime = Time.time;
        float endTime = startTime + 0.2f; // 200ms duration
        float startScale = obj.localScale.y;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / (endTime - startTime);
            float scaleValue = Mathf.Lerp(startScale, targetScale, t);
            obj.localScale = new Vector3(obj.localScale.x, scaleValue, obj.localScale.z);
            yield return null; // Wait for the next frame
        }
    }
}

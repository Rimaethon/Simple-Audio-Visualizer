using System.Collections;
using TMPro;
using UnityEngine;

public class TextRythm : MonoBehaviour
{
    public TextMeshProUGUI text; // Reference to the TextMeshPro object
    public float maxScale = 2f; // Maximum scale of the text
    public float scatterAmount = 100f; // Amount of scatter for each letter
    public float scatterSpeed = 0.1f; // Speed of the scatter animation
    public float rhythmInterval = 0.5f; // Interval of the rhythm animation
    public float rhythmScale = 1.5f; // Scale of the text during the rhythm animation

    private Vector3[] originalPositions; // Array to store the original positions of each letter
    private float timer = 0f; // Timer for the rhythm animation

    void Start()
    {
        // Get the original positions of each letter
        originalPositions = new Vector3[text.text.Length];
        for (int i = 0; i < text.text.Length; i++)
        {
            originalPositions[i] = text.textInfo.characterInfo[i].topLeft;
        }
    }

    void Update()
    {
        // Check if it's time for the next rhythm animation
        timer += Time.deltaTime;
        if (timer >= rhythmInterval)
        {
            StartCoroutine(RhythmAnimation());
            timer = 0f;
        }
    }

    IEnumerator RhythmAnimation()
    {
        // Scale up the text for the rhythm animation
        text.transform.localScale = new Vector3(rhythmScale, rhythmScale, 1f);

        // Scatter each letter of the text
        for (int i = 0; i < text.text.Length; i++)
        {
            // Get the current letter's position
            Vector3 position = text.textInfo.characterInfo[i].topLeft;

            // Scatter the letter in a random direction
            Vector3 scatterDirection = new Vector3(Random.Range(-scatterAmount, scatterAmount), Random.Range(-scatterAmount, scatterAmount), 0f);
            position += scatterDirection;

            // Animate the letter to its new position
            float t = 0f;
            while (t < scatterSpeed)
            {
                t += Time.deltaTime;
                text.textInfo.characterInfo[i].topLeft = Vector3.Lerp(originalPositions[i], position, t / scatterSpeed);
                text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                yield return null;
            }
        }

        // Scale down the text to its original size
        text.transform.localScale = new Vector3(1f, 1f, 1f);

        // Reset the positions of each letter
        for (int i = 0; i < text.text.Length; i++)
        {
            text.textInfo.characterInfo[i].topLeft = originalPositions[i];
        }
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}

using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public GameObject pixelPrefab; // Assign your pixel prefab here
    public int rows = 10; // Number of rows
    public Color colorStart = Color.red; // Start color of the gradient
    public Color colorEnd = Color.blue; // End color of the gradient
    public float spacing = 1.1f; // Spacing between pixels

    [SerializeField]
    private int activeRows = 5; // Number of active rows

    private GameObject[,] grid;

    void Start()
    {
        grid = new GameObject[rows, 3];

        for (int y = 0; y < rows; y++)
        {
            // Interpolate between start and end colors based on the row number
            Color pixelColor = Color.Lerp(colorStart, colorEnd, (float)y / rows);

            for (int x = 0; x < 3; x++)
            {
                var pixel = Instantiate(pixelPrefab, transform);
                pixel.transform.localPosition = new Vector3(x * spacing, y * spacing, 0);

                // Set the pixel's color
                var spriteRenderer = pixel.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = pixelColor;
                }

                grid[y, x] = pixel;

                // Deactivate the pixel if it's not in the active rows
                if (y >= activeRows)
                {
                    pixel.SetActive(false);
                }
            }
        }
    }

    // Call this method to add a row to the grid
    public void AddRow()
    {
        GameObject[,] newGrid = new GameObject[rows + 1, 3];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                newGrid[y, x] = grid[y, x];
            }
        }

        // Interpolate between start and end colors based on the new row number
        Color pixelColor = Color.Lerp(colorStart, colorEnd, (float)rows / (rows + 1));

        for (int x = 0; x < 3; x++)
        {
            var pixel = Instantiate(pixelPrefab, transform);
            pixel.transform.localPosition = new Vector3(x * spacing, rows * spacing, 0);

            // Set the pixel's color
            var spriteRenderer = pixel.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = pixelColor;
            }

            newGrid[rows, x] = pixel;

            // Deactivate the pixel if it's not in the active rows
            if (rows >= activeRows)
            {
                pixel.SetActive(false);
            }
        }

        grid = newGrid;
        rows++;
    }

    public void UpdateActiveRows(int newActiveRows)
    {
        // Update the activeRows variable
        activeRows = newActiveRows;

        // Activate the necessary rows and deactivate the rest
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                grid[y, x].SetActive(y < activeRows);
            }
        }
    }
}

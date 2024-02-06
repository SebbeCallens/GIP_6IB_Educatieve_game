using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slicer : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject _puzzlePiece;

    private GameObject PuzzlePiece { get => _puzzlePiece; set => _puzzlePiece = value; }

    public (int, int, float) SliceImage(Texture2D image, int maxColumns, int maxRows)
    {
        float imageAspect = (float)image.width / image.height;

        int columns, rows;

        if (image.width > image.height)
        {
            // Landscape image
            columns = Mathf.Min(maxColumns, image.width);
            rows = Mathf.RoundToInt(columns / imageAspect);
        }
        else
        {
            // Portrait image
            rows = Mathf.Min(maxRows, image.height);
            columns = Mathf.RoundToInt(rows * imageAspect);
        }

        float gridScale = 4.4f / (rows + 1);

        // Breedte en hoogte van 1 stuk
        int width = image.width / columns;
        int height = image.height / rows;

        // Calculate checkerboard pattern size based on image width divided by 20
        int checkerSize = image.width / 20;

        // Create a checkerboard pattern for the entire puzzle
        Color[] checkerboardPattern = GenerateCheckerboardPattern(checkerSize, checkerSize);

        // Voor elke rij
        for (int row = 0; row < rows; row++)
        {
            // Voor elke kolom
            for (int col = 0; col < columns; col++)
            {
                // Set up a new Texture2D with the specified width and height
                Texture2D texture = new Texture2D(width, height);

                // Get the pixels from the original image within the specified region
                Color[] pixels = image.GetPixels(col * width, row * height, width, height);

                // Iterate through each pixel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Check if the alpha value is less than a certain threshold (you can adjust this threshold as needed)
                        if (pixels[y * width + x].a < 0.5f)
                        {
                            // Use the checkerboard pattern for alpha pixels
                            int checkerX = (col * width + x) / checkerSize;
                            int checkerY = (row * height + y) / checkerSize;
                            bool isBlack = (checkerX + checkerY) % 2 == 0;
                            pixels[y * width + x] = isBlack ? Color.white : Color.white;
                        }
                    }
                }

                // Apply the modified pixels to the new texture
                texture.SetPixels(pixels);

                // Apply the changes to the texture
                texture.Apply();

                // Set the filter mode to Point for a pixelated look
                texture.filterMode = FilterMode.Point;

                // Het stuk instellen met gegeven positie
                GameObject imagePart = Instantiate(PuzzlePiece, Vector3.zero, Quaternion.identity, transform.GetChild(0).transform);

                // De schaal en sprite van het stuk instellen
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100);
                imagePart.GetComponent<Image>().sprite = sprite;
                imagePart.name = $"{IntToChar(col + 1)}-{rows - row}"; // Naam van afbeelding stuk instellen met coordinaten
            }
        }

        return (columns, rows, gridScale);
    }

    private Color[] GenerateCheckerboardPattern(int width, int height)
    {
        Color[] pattern = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool isBlack = (x + y) % 2 == 0;
                pattern[y * width + x] = isBlack ? Color.white : Color.white;
            }
        }

        return pattern;
    }

    public char IntToChar(int num)
    {
        return (char)('A' + num - 1);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.GetComponent<PuzzlePiece>() != null)
        {
            PuzzlePiece piece = dropped.GetComponent<PuzzlePiece>();
            piece.ParentAfterDrag = transform.GetChild(0).transform;
        }
    }
}

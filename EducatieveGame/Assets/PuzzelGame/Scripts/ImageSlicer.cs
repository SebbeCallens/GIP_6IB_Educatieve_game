using SFB;
using System.IO;
using UnityEngine;

public class ImageSlicer : MonoBehaviour
{
    [SerializeField] private GameObject _imagePart;
    [SerializeField] private float _scale;
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    private GameObject ImagePart { get => _imagePart; set => _imagePart = value; }
    private float Scale { get => _scale; set => _scale = value; }
    private int Width { get => _width; set => _width = value; }
    private int Height { get => _height; set => _height = value; }

    public void SelectImage()
    {
        string[] filePaths = StandaloneFileBrowser.OpenFilePanel("Select Image", "", "png", false);

        if (filePaths != null && filePaths.Length > 0 && !string.IsNullOrEmpty(filePaths[0]))
        {
            string imagePath = filePaths[0];

            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            SliceImage(texture);
        }
        else
        {
            Debug.LogError("Image selection canceled or failed.");
        }
    }

    private void SliceImage(Texture2D image)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int smallWidth = image.width / Width;
        int smallHeight = image.height / Height;
        float scale = 540f / image.height * Scale;
        int pixelsPerUnit = 100;
        float totalWidth = Width * smallWidth * scale / pixelsPerUnit;
        float totalHeight = Height * smallHeight * scale / pixelsPerUnit;

        float offsetX = -totalWidth / 2f + smallWidth * scale / pixelsPerUnit / 2f;
        float offsetY = -totalHeight / 2f + smallHeight * scale / pixelsPerUnit / 2f;

        transform.position = new Vector3(offsetX, offsetY, transform.position.z);

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                Texture2D smallTexture = new(smallWidth, smallHeight);
                Color[] pixels = image.GetPixels(col * smallWidth, row * smallHeight, smallWidth, smallHeight);

                smallTexture.SetPixels(pixels);
                smallTexture.Apply();
                smallTexture.filterMode = FilterMode.Point;

                float posX = transform.position.x + col * smallWidth * scale / pixelsPerUnit;
                float posY = transform.position.y + row * smallHeight * scale / pixelsPerUnit;
                Vector3 pos = new(posX, posY, transform.position.z);
                GameObject imagePart = Instantiate(ImagePart, pos, Quaternion.identity, transform);
                imagePart.transform.localScale = new(scale, scale, 1);
                Sprite smallSprite = Sprite.Create(smallTexture, new Rect(0, 0, smallWidth, smallHeight), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                imagePart.GetComponent<SpriteRenderer>().sprite = smallSprite;
            }
        }
    }
}

using UnityEngine;

public class GridManagerReference : MonoBehaviour
{
    [SerializeField] private ImageSlicer _slicer;
    [SerializeField] private Sprite _image;

    public ImageSlicer Slicer { get => _slicer; set => _slicer = value; }
    public Sprite Image { get => _image; set => _image = value; }

    private void Awake()
    {
        Texture2D texture = Image.texture;
        Slicer.SliceImage(texture);
    }
}

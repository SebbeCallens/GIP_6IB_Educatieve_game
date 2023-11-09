using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageArray : MonoBehaviour
{
    public Sprite[] _images;
    public Sprite _image0;
    public Sprite _image1;
    public Sprite _image2;
    public Sprite _image3;
    public Sprite _image4;
    public Sprite _image5;
    public Sprite _image6;

    private void Start()
    {
        _images = new Sprite[7];
        _images[0] = _image0;
        _images[1] = _image1;
        _images[2] = _image2;
        _images[3] = _image3;
        _images[4] = _image4;
        _images[5] = _image5;
        _images[6] = _image6;
    }
}

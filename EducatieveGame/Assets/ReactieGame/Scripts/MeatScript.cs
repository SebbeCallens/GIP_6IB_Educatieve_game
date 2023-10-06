using UnityEngine;
using UnityEngine.UI;

public class MeatScript : MonoBehaviour
{
    [SerializeField] private GameObject _mask;
    [SerializeField] private Gradient _meatColor;
    [SerializeField] private SpriteRenderer _meat;
    [SerializeField] private Sprite[] _meats;
    private Score _scoreObj;
    private ObjectSpawner _spawner;
    private float _rotationSpeed = -40f;

    private float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    private Gradient MeatColor { get => _meatColor; set => _meatColor = value; }
    private SpriteRenderer Meat { get => _meat; set => _meat = value; }
    private Sprite[] Meats { get => _meats; set => _meats = value; }
    private Score ScoreObj { get => _scoreObj; set => _scoreObj = value; }
    private ObjectSpawner Spawner { get => _spawner; set => _spawner = value; }
    private GameObject Mask { get => _mask; set => _mask = value; }

    private void Awake()
    {
        ScoreObj = GameObject.Find("Score").GetComponent<Score>();
        Spawner = GameObject.Find("Grid").GetComponent<ObjectSpawner>();
        int randomIndex = Random.Range(0, Meats.Length);
        Meat.sprite = Meats[randomIndex];
    }

    private void Update()
    {
        if (Mask.transform.eulerAngles.z > 45)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Mask.transform.rotation = Quaternion.Euler(Mask.transform.eulerAngles.x, Mask.transform.eulerAngles.y, Mask.transform.eulerAngles.z + RotationSpeed * Time.deltaTime);

            Meat.color = MeatColor.Evaluate((Mask.transform.eulerAngles.z - 45) / 180);

            if (Input.GetMouseButtonDown(0) && MouseInCell(mousePosition))
            {
                if (Mask.transform.eulerAngles.z > 190)
                {
                    ScoreObj.AddScore(-2);
                }
                else if (Mask.transform.eulerAngles.z > 135)
                {
                    ScoreObj.AddScore(-1);
                }
                else if (Mask.transform.eulerAngles.z < 75)
                {
                    ScoreObj.AddScore(-2);
                }
                else
                {
                     ScoreObj.AddScore(1);
                }

                Destroy(gameObject);
            }
        }
        else
        {
            ScoreObj.AddScore(-3);
            Destroy(gameObject);
        }
    }

    private bool MouseInCell(Vector3 position)
    {
        float minX = transform.position.x - 0.5f * Spawner.CellSize;
        float minY = transform.position.y - 0.5f * Spawner.CellSize;
        float maxX = transform.position.x + 0.5f * Spawner.CellSize;
        float maxY = transform.position.y + 0.5f * Spawner.CellSize;
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}

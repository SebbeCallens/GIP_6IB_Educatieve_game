using UnityEngine;

public class MeatScript : MonoBehaviour
{
    [SerializeField] private GameObject _mask; //de mask die over de kookmeter zit
    [SerializeField] private Gradient _meatColor; //de gradient kleur voor het vlees, van rauw naar verbrand
    [SerializeField] private SpriteRenderer _meat; //de spriterenderer van het vlees
    [SerializeField] private Sprite[] _meats; //alle mogelijke sprites voor het vlees
    private Stats _statsObj; //het scorescript
    private ObjectSpawner _spawner; //de spawner die het vlees spawned
    private float _rotationSpeed = -80f; //hoe vlug het vlees kookt

    private float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
    private Gradient MeatColor { get => _meatColor; set => _meatColor = value; }
    private SpriteRenderer Meat { get => _meat; set => _meat = value; }
    private Sprite[] Meats { get => _meats; set => _meats = value; }
    private Stats StatsObj { get => _statsObj; set => _statsObj = value; }
    private ObjectSpawner Spawner { get => _spawner; set => _spawner = value; }
    private GameObject Mask { get => _mask; set => _mask = value; }

    private void Awake() //random sprite instellen voor het vlees
    {
        StatsObj = GameObject.Find("Grid").GetComponent<Stats>();
        Spawner = GameObject.Find("Grid").GetComponent<ObjectSpawner>();
        int randomIndex = Random.Range(0, Meats.Length);
        Meat.sprite = Meats[randomIndex];

        //voor elk vlees een andere kooksnelheid
        if (Meat.sprite == Meats[0])
        {
            RotationSpeed *= 1 / (float)PlayerPrefs.GetInt("difficulty") * 1.4f;
        }
        else if (Meat.sprite == Meats[1])
        {
            RotationSpeed *= 1 / (float)PlayerPrefs.GetInt("difficulty") * 1.2f;
        }
        else if (Meat.sprite == Meats[2])
        {
            RotationSpeed *= 1 / (float)PlayerPrefs.GetInt("difficulty") * 1.6f;
        }
        else
        {
            RotationSpeed *= 1 / (float)PlayerPrefs.GetInt("difficulty");
        }
    }

    private void Update()
    {
        if (Mask.transform.eulerAngles.z > 45) //wordt uitegevoerd terwijl het vlees op de barbecue ligt
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Mask.transform.rotation = Quaternion.Euler(Mask.transform.eulerAngles.x, Mask.transform.eulerAngles.y, Mask.transform.eulerAngles.z + RotationSpeed * Time.deltaTime); //de mask over de kookmeter draaien

            Meat.color = MeatColor.Evaluate((Mask.transform.eulerAngles.z - 45) / 180); //de kleur instellen van het vlees

            if (Input.GetMouseButtonDown(0) && MouseInCell(mousePosition))
            {
                if (Mask.transform.eulerAngles.z > 190) //-2 score als het vlees nog diepgevroren is
                {
                    StatsObj.AddStat(3, 1);
                    StatsObj.AddStat(0, -2);
                }
                else if (Mask.transform.eulerAngles.z > 135) //-1 score wanneer het vlees nog rauw is
                {
                    StatsObj.AddStat(2, 1);
                    StatsObj.AddStat(0, -1);
                }
                else if (Mask.transform.eulerAngles.z < 75) //-1 score wanneer het vlees aangebrand is
                {
                    StatsObj.AddStat(4, 1);
                    StatsObj.AddStat(0, -1);
                }
                else //+1 score als het vlees goed gebakken is
                {
                    StatsObj.AddStat(1, 1);
                    StatsObj.AddStat(0, 1);
                }

                Destroy(gameObject);
            }
        }
        else //-3 score wanneer het vlees volledig opgebrand is
        {
            StatsObj.AddStat(5, 1);
            StatsObj.AddStat(0, -3);
            Destroy(gameObject);
        }
    }

    private bool MouseInCell(Vector3 position) //of de muis in de girdcel van het stuk vlees is
    {
        float minX = transform.position.x - 0.5f * Spawner.CellSize;
        float minY = transform.position.y - 0.5f * Spawner.CellSize;
        float maxX = transform.position.x + 0.5f * Spawner.CellSize;
        float maxY = transform.position.y + 0.5f * Spawner.CellSize;
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}

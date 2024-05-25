using TMPro;
using UnityEngine;

public class SortItem : MonoBehaviour
{
    [SerializeField] private Sprite[] _sortItems; //de verschillende sorteer object sprites
    private Vector3 _startPosition; //de startpositie van het sorteer object
    private Color _sortColor; //de kleur van het sorteer object
    private string _sortText; //de tekst van het sorteer object
    private bool _dragging = false; //of het sorteer object gesleept wordt
    private bool _onConveyor = false; //of het sorteer object op een loopband ligt
    private bool _isTrash = false; //of het sorteer object voor in de vuilbak is
    private Collider2D _bounds; //limiet speelveld
    private SortingGame _sortGame;
    private bool _sorting = false;

    private Sprite[] SortItems { get => _sortItems; set => _sortItems = value; }
    public Vector3 StartPosition { get => _startPosition; private set => _startPosition = value; }
    public Color SortColor { get => _sortColor; private set => _sortColor = value; }
    public string SortText { get => _sortText; private set => _sortText = value; }
    public bool Dragging { get => _dragging; set => _dragging = value; }
    private bool OnConveyor { get => _onConveyor; set => _onConveyor = value; }
    public bool IsTrash { get => _isTrash; private set => _isTrash = value; }
    private Collider2D Bounds { get => _bounds; set => _bounds = value; }
    private SortingGame SortGame { get => _sortGame; set => _sortGame = value; }
    public bool Sorting { get => _sorting; set => _sorting = value; }

    private void Awake() //start positie instellen
    {
        GetComponent<SpriteRenderer>().sprite = SortItems[Random.Range(0, SortItems.Length)];
        Bounds = GameObject.FindWithTag("Bounds").GetComponent<Collider2D>();
        StartPosition = transform.position;
        if (PlayerPrefs.GetInt("conveyor") == 1)
        {
            OnConveyor = true;
        }
    }

    private void Update() //sorteer object verplaatsen op loopband of verslepen
    {
        if (OnConveyor && !Dragging)
        {
            transform.position = new(transform.position.x + 0.6f * Time.deltaTime * MenuLogic.Difficulty, transform.position.y, transform.position.z);
        }

        if (Dragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; //z coordinaat niet veranderen

            //ervoor zorgen dat het sorteer object het spelveld niet verlaat
            Vector3 clampedPosition = new Vector3(Mathf.Clamp(mousePosition.x, Bounds.bounds.min.x, Bounds.bounds.max.x), Mathf.Clamp(mousePosition.y, Bounds.bounds.min.y, Bounds.bounds.max.y), mousePosition.z);

            transform.position = clampedPosition;
        }
    }

    private void OnMouseDown() //begin slepen sorteer object
    {
        if (enabled)
        {
            Dragging = true;
            SortGame.Dragging = true;
            if (OnConveyor)
            {
                StartPosition = transform.position;
            }
        }
    }

    private void OnMouseUp() //eind slepen sorteer object
    {
        if (enabled)
        {
            if (Dragging)
            {
                Dragging = false;
                SortGame.Dragging = false;
                if (OnConveyor)
                {
                    transform.position = StartPosition;
                }
            }
        }
    }

    public void Create(Color color, string text, bool isTrash, bool onConveyor, SortingGame sortGame) //sorteer object aanmaken
    {
        SortColor = color;
        SortText = text;
        IsTrash = isTrash;
        OnConveyor = onConveyor;
        transform.GetComponentInChildren<TextMeshPro>().color = SortColor;
        transform.GetComponentInChildren<TextMeshPro>().text = SortText;
        SortGame = sortGame;
    }
}

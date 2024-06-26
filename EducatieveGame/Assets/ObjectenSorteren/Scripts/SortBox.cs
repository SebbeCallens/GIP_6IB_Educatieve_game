using System.Collections;
using UnityEngine;

public class SortBox : MonoBehaviour
{
    private SortingGame _sortGame; //script sorteer spel
    private Color _sortColor; //de kleur van deze doos
    private string _sortText; //de tekst van de kleur van deze doos
    private bool _isTrashcan; //of dit de vuilbak is
    [SerializeField] private GameObject _burnItemParticle;
    [SerializeField] private GameObject[] _sortedItemParticles;
    [SerializeField] private GameObject _lostItemParticle;

    private SortingGame SortGame { get => _sortGame; set => _sortGame = value; }
    private Color SortColor { get => _sortColor; set => _sortColor = value; }
    private string SortText { get => _sortText; set => _sortText = value; }
    private bool IsTrashcan { get => _isTrashcan; set => _isTrashcan = value; }

    private void Awake() //componenten instellen
    {
        SortGame = GameObject.FindWithTag("SortingGame").GetComponent<SortingGame>();
    }

    private void OnTriggerEnter2D(Collider2D collision) //een object raakt de doos
    {
        if (collision.gameObject.CompareTag("SortItem")) //nakijken of dit een sorteerobject is
        {
            if (collision.GetComponent<SortItem>().Dragging)
            {
                if (!(!SortGame.TrashcanMode && IsTrashcan))
                {
                    StartCoroutine(CheckSortItem(collision.GetComponent<SortItem>(), collision));
                }
            }
            else
            {
                if (collision.GetComponent<SortItem>().IsTrash)
                {
                    SortGame.ItemSorted();
                }
                else
                {
                    SortGame.ItemLost();
                }
                Instantiate(_burnItemParticle, new Vector3(collision.gameObject.transform.position.x + 2, collision.gameObject.transform.position.y - 0.25f, collision.gameObject.transform.position.z), collision.gameObject.transform.rotation);
                Destroy(collision.gameObject);
            }
        }
    }

    public void Create(Color color, string text, bool isTrashcan) //de sorteer doos instellen
    {
        SortColor = color;
        SortText = text;
        IsTrashcan = isTrashcan;
        GetComponent<SpriteRenderer>().color = color;
    }

    private IEnumerator CheckSortItem(SortItem item, Collider2D collision)
    {
        while (item.Dragging)
        {
            yield return null;
        }

        if (GetComponent<Collider2D>().IsTouching(collision) && !item.Sorting)
        {
            item.Sorting = true;
            if (IsTrashcan) //code als dit een vuilbak is
            {
                if (collision.GetComponent<SortItem>().IsTrash)
                {
                    SortGame.ItemSorted();
                    Instantiate(_burnItemParticle, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    Destroy(collision.gameObject);
                }
                else
                {
                    SortGame.ItemLost();
                    collision.gameObject.GetComponent<SortItem>().Dragging = false;
                    collision.gameObject.transform.position = collision.gameObject.GetComponent<SortItem>().StartPosition;
                }
            }
            else //code als dit geen vuilbak is
            {
                if (SortGame.SortByColor)
                {
                    if (collision.GetComponent<SortItem>().SortColor == SortColor)
                    {
                        SortGame.ItemSorted();
                        GameObject sortedItemParticle = _sortedItemParticles[Random.Range(0, _sortedItemParticles.Length)];
                        Instantiate(sortedItemParticle, transform.position, transform.rotation);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        SortGame.ItemLost();
                        Instantiate(_lostItemParticle, transform.position, transform.rotation);
                        collision.gameObject.GetComponent<SortItem>().Dragging = false;
                        collision.gameObject.transform.position = collision.gameObject.GetComponent<SortItem>().StartPosition;
                    }
                }
                else
                {
                    if (collision.GetComponent<SortItem>().SortText == SortText)
                    {
                        SortGame.ItemSorted();
                        GameObject sortedItemParticle = _sortedItemParticles[Random.Range(0, _sortedItemParticles.Length)];
                        Instantiate(sortedItemParticle, transform.position, transform.rotation);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        SortGame.ItemLost();
                        Instantiate(_lostItemParticle, transform.position, transform.rotation);
                        collision.gameObject.GetComponent<SortItem>().Dragging = false;
                        collision.gameObject.transform.position = collision.gameObject.GetComponent<SortItem>().StartPosition;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
            item.Sorting = false;
        }
    }
}

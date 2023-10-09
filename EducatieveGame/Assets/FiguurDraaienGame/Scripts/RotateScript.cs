using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private RotateGame _game;

    private RotateGame Game { get => _game; set => _game = value; }

    private void Awake()
    {
        Game = GameObject.Find("GameLogic").GetComponent<RotateGame>();
    }

    private void Update() //de gridcel 90 graden draaien wanneer de muis erop klikt
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && MouseInCell(mousePosition) && Game.GameInProgress)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90f);
        }
    }

    private bool MouseInCell(Vector3 position) //of de muis in de girdcel van het stuk vlees is
    {
        float minX = transform.position.x - 0.5f * Game.CellSize;
        float minY = transform.position.y - 0.5f * Game.CellSize;
        float maxX = transform.position.x + 0.5f * Game.CellSize;
        float maxY = transform.position.y + 0.5f * Game.CellSize;
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}

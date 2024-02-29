using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private RotateGame _game; //script van het spel

    private RotateGame Game { get => _game; set => _game = value; }

    private void Awake() //componenten instellen
    {
        Game = GameObject.Find("GameLogic").GetComponent<RotateGame>();
    }

    private void OnMouseDown() //cel draaien wanneer de muis er op klikt
    {
        if (Game.GameInProgress && Time.timeScale != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90f);
        }
    }
}

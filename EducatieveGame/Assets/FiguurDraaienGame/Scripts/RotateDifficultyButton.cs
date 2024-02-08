using UnityEngine;

public class RotateDifficultyButton : MonoBehaviour
{
    public void SetDifficulty(int difficulty)
    {
        MenuLogic.SetDifficulty(difficulty);
    }
}

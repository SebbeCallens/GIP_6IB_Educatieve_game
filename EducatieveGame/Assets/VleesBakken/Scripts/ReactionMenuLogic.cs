using UnityEngine;

public class ReactionMenuLogic : MenuLogic
{
    private void Awake()
    {
        AwakeBase();
        if (ResetDifficulty)
        {
            PlayerPrefs.SetInt("meat", 1);
            PlayerPrefs.SetInt("rate", 1);
            PlayerPrefs.SetInt("size", 1);
        }
    }
}

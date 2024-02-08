using UnityEngine;

public class PathMenuLogic : MenuLogic
{
    private void Awake()
    {
        AwakeBase();
    }

    public override void ToggleSetting(int index) //instelling bijwerken
    {
        if (!FromScript)
        {
            if (index != 2 && PlayerPrefs.GetInt(Settings[2]) == 1)
            {
                FromScript = true;
                PlayerPrefs.SetInt(Settings[2], 0);
                SettingsToggles[2].isOn = false;
                FromScript = false;
            }
            else if (index == 2)
            {
                if (PlayerPrefs.GetInt(Settings[0]) == 1)
                {
                    FromScript = true;
                    PlayerPrefs.SetInt(Settings[0], 0);
                    SettingsToggles[0].isOn = false;
                    FromScript = false;
                }
                if (PlayerPrefs.GetInt(Settings[1]) == 1)
                {
                    FromScript = true;
                    PlayerPrefs.SetInt(Settings[1], 0);
                    SettingsToggles[1].isOn = false;
                    FromScript = false;
                }
            }

            if (PlayerPrefs.GetInt(Settings[index]) == 1)
            {
                PlayerPrefs.SetInt(Settings[index], 0);
            }
            else
            {
                PlayerPrefs.SetInt(Settings[index], 1);
            }
        }
    }
}

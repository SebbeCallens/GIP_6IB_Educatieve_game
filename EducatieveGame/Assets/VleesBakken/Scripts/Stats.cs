using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _statTexts; //lijst met text objecten van de statistieken
    private string[] _statNames; //lijst met de namen van de statistieken
    private int[] _statValues; //lijst me de waarden van de statistieken

    private TextMeshProUGUI[] StatTexts { get => _statTexts; set => _statTexts = value; }
    public string[] StatNames { get => _statNames; private set => _statNames = value; }
    public int[] StatValues { get => _statValues; private set => _statValues = value; }

    private void Awake() //statistieken instellen op 0
    {
        StatValues = new int[StatTexts.Length];
        StatNames = new string[StatTexts.Length];
        for (int i = 0; i < StatTexts.Length; i++)
        {
            StatNames[i] = StatTexts[i].text;
            StatValues[i] = 0;
            AddStat(i, 0);
        }
    }

    public void AddStat(int index, int value) //statistiek bijwerken op gegeven index
    {
        StatValues[index] += value;
        StatTexts[index].text = StatNames[index] + ": " + StatValues[index].ToString();
    }
}

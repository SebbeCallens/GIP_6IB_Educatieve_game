using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreens : MonoBehaviour
{
    [SerializeField] private RectTransform _gameTitle;
    [SerializeField] private RectTransform _statsTitle;
    [SerializeField] private RectTransform _scoreTitle;
    [SerializeField] private RectTransform _difficultyTitle;
    [SerializeField] private RectTransform _score;
    [SerializeField] private RectTransform _gameStats;
    [SerializeField] private RectTransform _preview;

    private RectTransform GameTitle { get { return _gameTitle; } }
    private RectTransform StatsTitle { get { return _statsTitle;} }
    private RectTransform ScoreTitle { get { return _scoreTitle;} }
    private RectTransform DifficultyTitle { get { return _difficultyTitle;} }
    private RectTransform Score {  get { return _score;} }
    private RectTransform GameStats { get { return _gameStats; } }
    private RectTransform Preview { get { return _preview; } }

    public void Rearrange(int spel)
    {
        gameObject.transform.GetChild(spel).gameObject.SetActive(true);
        GameTitle.rect.Set(0f, -290f, 660.5f, 60);
        switch (spel)
        {
            case 0:
                ObjSort();
                break;
            case 1:
                FigDraai();
                break;
            case 2:
                VleesBak();
                break;
            case 3:
                CoordPuzz();
                break;
            case 4:
                PadVolg();
                break;
            default:
                FigTeken();
                break;
        }
    }

    //Plaatst alles in de juiste positie
    private void ObjSort()
    {
        StatsTitle.rect.Set(205f, -360f, 550f, 100);
        ScoreTitle.rect.Set(-164.5f, -450f, 600f, 100f);
        DifficultyTitle.rect.Set(-164.5f, -760f, 600f, 100f);
        Score.rect.Set(-164.5f, -833f, 600f, 600f);
        GameStats.rect.Set(270f, -150f, 300.5f, 630f);
        Preview.rect.Set(0f, 360f, 960f, 580f);
    }
    private void FigDraai()
    {
        StatsTitle.rect.Set(205f, -360f, 550f, 100);
        ScoreTitle.rect.Set(-175f, -415f, 600f, 100f);
        DifficultyTitle.rect.Set(195f, -895f, 600f, 100f);
        Score.rect.Set(-175f, -803f, 600f, 600f);
        GameStats.rect.Set(240f, -96f, 330f, 465f);
        Debug.Log("Code Uitgevoert");
    }
    private void VleesBak()
    {
        StatsTitle.rect.Set(0f, 360f, 550f, 100);
        ScoreTitle.rect.Set(245.5f, -450f, 600f, 100f);
        DifficultyTitle.rect.Set(-164.5f, -445f, 600f, 100f);
        Score.rect.Set(270.5f, -833f, 600f, 600f);
        GameStats.rect.Set(0f, 700f, 330f, 210f);
        Preview.rect.Set(0f, 360f, 960f, 580f);
    }
    private void CoordPuzz()
    {
        StatsTitle.rect.Set(0f, 360f, 550f, 100);
        ScoreTitle.rect.Set(-230f, -390f, 600f, 100f);
        DifficultyTitle.rect.Set(180f, -430f, 600f, 100f);
        Score.rect.Set(-230f, -775f, 600f, 600f);
        GameStats.rect.Set(0f, 200f, 330f, 210f);
        Preview.rect.Set(0f, -301.5f, 960f, 583f);
    }
    private void PadVolg()
    {
        StatsTitle.rect.Set(205f, -360f, 550f, 100);
        ScoreTitle.rect.Set(-180f, -370f, 600f, 100f);
        DifficultyTitle.rect.Set(-180f, -565f, 600f, 100f);
        Score.rect.Set(-164.5f, -740f, 600f, 600f);
        GameStats.rect.Set(240f, -195f, 330f, 660f);
    }
    private void FigTeken()
    {
        StatsTitle.rect.Set(0f, 360f, 550f, 100);
        ScoreTitle.rect.Set(-164.5f, -450f, 600f, 100f);
        DifficultyTitle.rect.Set(-164.5f, -760f, 600f, 100f);
        Score.rect.Set(-164.5f, -833f, 600f, 600f);
        GameStats.rect.Set(0f, 700f, 330f, 210f);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public List<Image> starList;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI cristalText;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameManager.instance.OnGameFinished += Gamemanager_OnGameFinished;
    }

    private void Gamemanager_OnGameFinished(int score)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        int starNumber = GameManager.instance.GetScoreStar();
        for (int i = 0; i < starNumber; i++)
        {
            starList[i].color = Resources.Load<ColorSO>("FilledStar_" + typeof(ColorSO).Name).color;
        }
        scoreText.text = score.ToString();
        cristalText.text = GameManager.instance.GetCristalValue().ToString();
    }
}
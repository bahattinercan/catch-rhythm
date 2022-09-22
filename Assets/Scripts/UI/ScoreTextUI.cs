using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextUI : MonoBehaviour
{
    
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        GameManager.instance.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int val)
    {
        text.text = val.ToString();
    }
}

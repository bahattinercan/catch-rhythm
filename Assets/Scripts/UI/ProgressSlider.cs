using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        GameManager.instance.OnScoreChanged += OnScoreChanged;
        slider.maxValue=GameManager.instance.GetMaxGoal();
    }

    private void OnScoreChanged(int score)
    {
        slider.value = score;
    }
}
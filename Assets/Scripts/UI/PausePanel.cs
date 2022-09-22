using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private GameObject pauseButton;
    private GameObject pausePanel;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "panel")
                pausePanel = child.gameObject;
            else if (child.name == "pauseButton")
                pauseButton = child.gameObject;
        }
        pausePanel.SetActive(false);
        GameManager.instance.OnGameFinished += GameManager_OnGameFinished;
    }

    private void GameManager_OnGameFinished(int obj)
    {
        Hide();
    }

    public void Pause()
    {
        GameManager.instance.PauseGame();
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        GameManager.instance.ResumeGame();
        pauseButton.SetActive(true);
        pausePanel.SetActive(false);
    }

    private void Hide()
    {
        pauseButton.SetActive(false);
        pausePanel.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public int totalLevels = 3;
    
    public GameObject levelCompletedPanel;
    public GameObject gameOverPanel;
    void Start()
    {
        levelCompletedPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        int savedLevelIndex = PlayerPrefs.GetInt("LevelIndex", 0);
    }
    
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void LevelCompleted()
    {
        levelCompletedPanel.SetActive(true);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextButton()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("LevelIndex", 0);
        currentLevelIndex = (currentLevelIndex + 1) % totalLevels;

        PlayerPrefs.SetInt("LevelIndex", currentLevelIndex);
        
        SceneManager.LoadScene(currentLevelIndex);
    }
}

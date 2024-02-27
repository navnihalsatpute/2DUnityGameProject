using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button[] buttons;
    public GameObject pauseMenuScreen;
    public GameObject loadMenuScreen;
    public GameObject mobileControls;
    public GameObject pausebtn;
    private int initialindex = 1;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i=0; i<buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for(int i=0; i<unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(initialindex);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadGame()
    {
        loadMenuScreen.SetActive(true);
    }
    public void back()
    {
        loadMenuScreen.SetActive(false);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuScreen.SetActive(true);
        mobileControls.SetActive(false);
        pausebtn.SetActive(false);
    }
    public void OpenLevel(int LevelId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(LevelId);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuScreen.SetActive(false);
        mobileControls.SetActive(true);
        pausebtn.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Start Game");
    }
}

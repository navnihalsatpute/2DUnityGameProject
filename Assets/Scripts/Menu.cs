using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject pauseMenuScreen;
    private int initialindex = 1;
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
        SceneManager.LoadScene("Load Game");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void back()
    {
        SceneManager.LoadScene("Start Game");
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuScreen.SetActive(true);
    }
    public void Load1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(initialindex);
    }
    public void Load2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(initialindex+1);
    }
    public void Load3()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(initialindex+2);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuScreen.SetActive(false);
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

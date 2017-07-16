using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script handles the main menu behavior (mainly changing scenes on button click)
/// </summary>
public class MainMenuHandler : MonoBehaviour {

    public string LevelSelection = "Menu_LevelSelection";

    public void Update()
    {
        Camera.main.backgroundColor = new Color(0, 0, 0, 0);

    }
    public void GoToLevelSelection()
    {
        SceneManager.LoadScene(LevelSelection, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

}

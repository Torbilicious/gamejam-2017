using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Load Levels
public class LevelSelectionHandler : MonoBehaviour {

    public string MainMenu = "Main_Menu";
    public string Level = "Level";

	public void GoToMainMenu()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(Level, LoadSceneMode.Single);
    }

    public void OnButtonLevel1Click()
    {
        LevelManager.LevelName = "level";
        LoadLevel();
    }
}

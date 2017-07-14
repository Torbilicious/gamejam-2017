using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Load Levels
public class LevelSelectionHandler : MonoBehaviour {

    public string MainMenu = "Main_Menu";

	public void GoToMainMenu()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
    }
}

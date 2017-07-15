using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Load Levels
public class LevelSelectionHandler : MonoBehaviour
{
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
        LevelManager.Instance.LevelName = "level1";
        LoadLevel();
    }

    public void OnButtonLevel2Click()
    {
        LevelManager.Instance.LevelName = "level2";
        LoadLevel();
    }

    public void OnButtonLevel3Click()
    {
        LevelManager.Instance.LevelName = "level3";
        LoadLevel();
    }

    public void OnButtonLevel4Click()
    {
        //LevelManager.Instance.LevelName = "level4";
        //LoadLevel();
    }

    public void OnButtonLevel5Click()
    {
        //LevelManager.Instance.LevelName = "level5";
        //LoadLevel();
    }

    public void OnButtonLevel6Click()
    {
        //LevelManager.Instance.LevelName = "level6";
        //LoadLevel();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This is the main class for a level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private bool _levelLoaded = false;
    private int _lemmingCount;
    public static LevelManager Instance;
    public static string LevelName = "level1";

    public static int Points = 0;

    public static bool FireAlarmTriggered = false;

    public LevelModel LevelModel;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private UnityEngine.UI.Text score;

    public void StartLevel()
    {
        List<Lemming> lemmingAIs = new List<Lemming>(GameObject.FindObjectsOfType<Lemming>());
        lemmingAIs.ForEach((Lemming l) => { l.StartAI(); });

        new List<FireSource>(FindObjectsOfType<FireSource>()).ForEach(s => s.GameStarted());
        gameStarted = true;
    }

    private bool gameStarted = false;

    public void ReloadLevel()
    {
        //SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
        Points = 0;
        Application.LoadLevel(Application.loadedLevel);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        List<Lemming> lemmingAIs = new List<Lemming>(GameObject.FindObjectsOfType<Lemming>());
        _lemmingCount = lemmingAIs.Count - 1;
    }

    public GameObject gameOverPanel;

    private void CheckGameFinished()
    {
        if (gameStarted && GameObject.FindObjectsOfType<Lemming>().Length <= 1)
        {
            int score = Points - ((_lemmingCount * 10) / 8);
            gameOverPanel.transform.GetComponentInChildren<Text>().text = score < 0 ? "You loose!" : "You win!\nBonus: " + score;
            gameOverPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGameFinished();

        //Load the level
        if (!_levelLoaded && ModelStore.Instance != null)
        {
            _levelLoaded = true;
            TextAsset levelFile = Resources.Load("Levels/" + LevelName) as TextAsset;
            LevelModel.Deserialize(levelFile.text);
        }

        if (_levelLoaded)
        {
            score.text = string.Format("{0}/{1}", Points, _lemmingCount);
        }
    }

    public void Exit2LevelMenu()
    {
        SceneManager.LoadScene("Menu_LevelSelection");
    }
}
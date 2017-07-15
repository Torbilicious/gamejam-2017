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
    public static string LevelName = "level";

    public static int Points = 0;
    private int minLemmingCount = 0;

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
        minLemmingCount = lemmingAIs.Count * 10 / 8;
        lemmingAIs.ForEach((Lemming l) => { l.StartAI(); });
        gameStarted = true;
    }

    private bool gameStarted = false;

    public void ReloadLevel()
    {
        SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
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
            int score = Points - minLemmingCount;
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

            Transform lemming = Instantiate(ModelStore.Instance.baseLemming);
            lemming.position = new Vector3(0, 1, 0);

            Transform lemming2 = Instantiate(ModelStore.Instance.baseLemming);
            lemming2.position = new Vector3(1, 1, 0);
            Transform lemming3 = Instantiate(ModelStore.Instance.baseLemming);
            lemming3.position = new Vector3(2, 1, 0);
        }

        if(_levelLoaded)
        {
            score.text = string.Format("{0}/{1}", Points, _lemmingCount);
        }
    }

    public void Exit2LevelMenu()
    {
        SceneManager.LoadScene("Menu_LevelSelection");
    }
}
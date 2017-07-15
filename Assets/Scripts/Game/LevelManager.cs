using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main class for a level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private bool _levelLoaded = false;

    public static string LevelName = "level";

    public LevelModel LevelModel;

    private void Awake()
    {
    }

    [SerializeField]
    private UnityEngine.UI.Text score;

    public void StartLevel()
    {
        List<LemmingAI> lemmingAIs = new List<LemmingAI>(GameObject.FindObjectsOfType<LemmingAI>());
        lemmingAIs.ForEach((LemmingAI l) => { l.StartAI(); });
    }

    public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        List<LemmingAI> lemmingAIs = new List<LemmingAI>(GameObject.FindObjectsOfType<LemmingAI>());
        score.text = string.Format("{0}/{0}", lemmingAIs.Count, lemmingAIs.Count);
    }

    // Update is called once per frame
    private void Update()
    {
        //Load the level
        if (!_levelLoaded && ModelStore.Instance != null)
        {
            _levelLoaded = true;
            TextAsset levelFile = Resources.Load("Levels/" + LevelName) as TextAsset;
            LevelModel.Deserialize(levelFile.text);

            Transform lemming = Instantiate(ModelStore.Instance.baseLemming);
            lemming.position = new Vector3(0, 1, 0);

            Transform lemming2 = Instantiate(ModelStore.Instance.baseLemming);
            lemming2.position = new Vector3(2, 1, 0);
        }
    }
}
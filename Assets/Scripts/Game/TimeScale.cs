using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    [SerializeField]
    private float timeScaleFactor = 4;

    [SerializeField]
    private Sprite textureNormal, textureFastMode;

    [SerializeField]
    private UnityEngine.UI.Image buttonImage;

    private bool fastMode = false;

    public void ToggleTimeScale()
    {
        fastMode = !fastMode;
        Time.timeScale = fastMode ? timeScaleFactor : 1;
        buttonImage.sprite = !fastMode ? textureFastMode : textureNormal;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }
}
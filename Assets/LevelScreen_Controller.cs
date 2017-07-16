using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScreen_Controller : MonoBehaviour {

    void Start()
    {
        GetComponent<Animator>().SetBool("onFire", true);
        Time.timeScale = 0.5f;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

	void Start () {
        GetComponent<Animator>().SetBool("extinguishing",true);
        Time.timeScale = 0.5f;
    }
	
    void OnDisable()
    {
        Time.timeScale = 1;
    }

}

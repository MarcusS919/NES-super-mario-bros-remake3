using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
    
    CanvasGroup cg;
    public Button btnPause;

	// Use this for initialization
	void Start () {
        cg = GetComponent<CanvasGroup>();
        if (!cg)
            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0.0f;

        if(btnPause)
            btnPause.onClick.AddListener(PauseGame);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
	}

    public void PauseGame()
    {
        if(cg.alpha == 0.0f)
        {
            cg.alpha = 1.0f;

            Time.timeScale = 0.0f;
        }
        else
        {
            cg.alpha = 0.0f;

            Time.timeScale = 1.0f;
        }
    }
}

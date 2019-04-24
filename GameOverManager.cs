using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    CanvasGroup cg;
    public Button btnRestart;
    public Button btnQuit;

    // Use this for initialization
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        if (!cg)
            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0.0f;

        if (btnRestart)
            btnRestart.onClick.AddListener(RestartGame);

        if (btnQuit)
            btnRestart.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

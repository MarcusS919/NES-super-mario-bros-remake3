using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NullState, MainMenu, Game }

public class GameManager : MonoBehaviour
{

    static GameManager _instance = null;
    int _lives;

    GameState _gm = GameState.NullState;

    public GameObject playerPrefab;

    // Use this for initialization
    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;

            DontDestroyOnLoad(this);
        }

        lives = 3;

        gm = GameState.MainMenu;

        PlayerPrefs.SetString("PlayerName", "Tom");
        PlayerPrefs.SetFloat("Brightness", 0.5f);
        PlayerPrefs.SetInt("LastLevel", 2);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        gm = GameState.Game;
        SceneManager.LoadScene("Level1");
        //SceneManager.LoadScene(1);
        //Invoke("LoadLevel", 1.0f);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        if (playerPrefab && spawnLocation)
        {
            Instantiate(playerPrefab, spawnLocation.position,
                spawnLocation.rotation);
        }
    }

    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public int lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            if (gm == GameState.Game)
            {
                if (_lives > 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                else
                    Debug.Log("Player Dead");
            }
        }
    }

    public GameState gm
    {
        get { return _gm; }
        set
        {
            _gm = value;
            Debug.Log("Current State: " + _gm);
        }
    }
}

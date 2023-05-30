using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public static class GameSettings
{
    public static int Difficulty = 2; // Default difficulty level is 2 (Medium)
}
public class GameManager : MonoBehaviour
{
    #region Variables
    public int numberOfenemys;
    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;
    public GameObject PlayerSpawnPointAtStart;
    public Transform PlayerSpawnPoint;
    public RecoverySpawner recoverySpawner;
    //Spawn variables
    public GameObject[] recoveryPoints;
    public Transform Player_prefab;
    public GameObject[] determinedLocation;
    public GameObject playerObject;
    public string PlayerSpawnPointTag = "PlayerSpawners";
    public string PlayerAtStartTag = "PlayerSpawnerAtStart";
    //Score variables
    public float p_current_health = 10;
    public float deathPenalty = 50;
    public float currentScore = 0;
    public Text currentScoreText;

    public GameObject[] terminals;
    public GameObject[] tiles;

    //Access to other Scripts
    public PlayerManager playerManager;
    public RecoveryBox recoveryBox;
    public enemyManager enemyManager;
    public enemySpawner enemySpawner;
    public WeaponManager weaponManager;
    //public GameObject enemyAI;

    //UI some of them are not assigned yet
    public GameObject endScreen;
    public float endScore;
    public Text endScoreText;
    public Text destroyedText;
    //public int enemiesKilled;
    //public Text enemiesKilledNumber;
    public GameObject pauseMenu;

    //DDA
    public int currentDifficulty;    
    public int[] initialEnemiesByRoom;    
    public float spawnDelay;
    public float enemyCurrentHealth;
    public float recoveryAmount;
    private float currentRecoveryAmount;
    public int[] initialAidsByRoom;
    public static GameManager instance;
    #endregion

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one GameManager instance exists
        }

    }
    void Start()
    {
        //SETTING DIFFICULTY
        SetDifficultyLevel(2); //default
        //SetDifficultyLevel(1);
        Cursor.lockState = CursorLockMode.None;

        //recoveryBox = GameObject.FindWithTag("Recovery").GetComponent<RecoveryBox>();        
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawners");
        recoveryPoints = GameObject.FindGameObjectsWithTag("RecoverySpawners");
        //playerObject = (GameObject)(Resources.Load("Player"));
        enemyPrefab = (GameObject)(Resources.Load("enemyAI"));
        PlayerSpawnPointAtStart = GameObject.FindGameObjectWithTag("PlayerSpawnerAtStart");
        
        // Setting a random terminal (Room 7-8-9)
        int randomIndex = UnityEngine.Random.Range(0, terminals.Length);
        terminals[randomIndex].SetActive(true);
        // Terminal tile color change
        foreach (GameObject tile in tiles)
        {
            if (tile.GetComponent<Collider>().bounds.Intersects(terminals[randomIndex].GetComponent<Collider>().bounds))
            {
                tile.GetComponent<Renderer>().material.color = new Color32(80, 15, 15, 255);
            }
        }
    }

    void Update()
    {
        //GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject playerObject = GameObject.Find("Player");
            PlayerManager playerManager = playerObject.GetComponent<PlayerManager>();
            Debug.Log("Player health: " + playerManager.p_current_health);
            Debug.Log("Number of deaths: " + playerManager.numberofDeath);

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Access to enemyManager script
            GameObject enemyAI = GameObject.FindGameObjectWithTag("enemy");
            enemyManager enemyManager = enemyAI.GetComponent<enemyManager>();

            //Access to enemySpawner script
            GameObject EnemySpawner = GameObject.Find("EnemySpawner");
            enemySpawner enemySpawner = EnemySpawner.GetComponent<enemySpawner>();
            //Access to RecoveryBox script
            GameObject RecoveryBox = GameObject.Find("RecoveryBoxObject");
            RecoveryBox recoveryBox = RecoveryBox.GetComponent<RecoveryBox>();
            //Access to RecoverySpawner script
            GameObject RecoverySpawner = GameObject.Find("RecoverySpawnPoints");
            RecoverySpawner recoverySpawner = RecoverySpawner.GetComponent<RecoverySpawner>();
        }
        //Access to WeaponManager script
        GameObject WeaponHolder = GameObject.FindGameObjectWithTag("Weapon"); //pistol shotgun rifle
        WeaponManager weaponManager = WeaponHolder.GetComponent<WeaponManager>();
        //destroyedText.text = weaponManager.numberofDestroyed.ToString();

    }

    public void EndGame()
    {
        Debug.Log("the end");
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        endScreen.SetActive(true);
        endScore = playerManager.Score;
        endScoreText.text = endScore.ToString();
        Application.Quit();
    }

    public void SetDifficultyLevel(int difficulty)
    {
        currentDifficulty = difficulty;

        // Adjust variables based on difficulty level
        switch (currentDifficulty)
        {
            case 0: // EASY L0
                initialAidsByRoom = new int[] { 3, 4, 3, 3, 3, 3, 4, 4, 4 };
                currentRecoveryAmount = 50f;
                initialEnemiesByRoom = new int[] { 1, 2, 2, 1, 1, 1, 1, 2, 3 };
                spawnDelay = 6f;
                enemyCurrentHealth = 60f;
                break;
            case 1: // L1
                initialAidsByRoom = new int[] { 3, 2, 3, 3, 2, 3, 3, 2, 3 };
                currentRecoveryAmount = 45f;
                initialEnemiesByRoom = new int[] { 2, 1, 2, 2, 2, 2, 1, 3, 4 };
                spawnDelay = 5f;
                enemyCurrentHealth = 80f;
                break;
            case 2: // DEFAULT L2
                initialAidsByRoom = new int[] { 2, 2, 2, 2, 2, 2, 3, 3, 3 };
                currentRecoveryAmount = 40f;
                initialEnemiesByRoom = new int[] { 2, 2, 3, 2, 3, 2, 2, 4, 5 };
                spawnDelay = 5f;
                enemyCurrentHealth = 100f;
                break;
            case 3: // L3
                initialAidsByRoom = new int[] { 2, 1, 2, 2, 1, 2, 2, 1, 2 };
                currentRecoveryAmount = 35f;
                initialEnemiesByRoom = new int[] { 3, 3, 4, 3, 3, 3, 3, 5, 6 };
                spawnDelay = 2f;
                enemyCurrentHealth = 120f;
                break;
            case 4: // HARD L4
                initialAidsByRoom = new int[] { 1, 1, 1, 2, 1, 2, 1, 1, 1 };
                currentRecoveryAmount = 25f;
                initialEnemiesByRoom = new int[] { 4, 3, 5, 4, 4, 4, 4, 5, 6 };
                spawnDelay = 1f;
                enemyCurrentHealth = 140f;
                break;
        }
        recoveryAmount = currentRecoveryAmount;
        //Debug.Log("spawn delay is" + spawnDelay);
        
    }


    /*void AdjustGameDifficulty()
    {
        int difficulty = GameSettings.Difficulty;

        switch (difficulty)
        {
            case 0:
                
                break;
            case 1:
                
                break;
            case 2:
                
                break;
            case 3:
                
                break;
            case 4:
                
                break;
            default:
                Debug.Log("Unknown difficulty level");
                break;
        }
    }

    void IncreaseDifficulty()
    {
        GameSettings.Difficulty++;
        if (GameSettings.Difficulty > 4)
        {
            GameSettings.Difficulty = 4;
        }
    }*/
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = 0;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.volume = 1;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        //blackScreenAnimator.SetTrigger("FadeIn");
        Invoke("LoadMainMenuScene", 0.4f);
    }
    public void LoadMainMenuScene()
    {
        AudioListener.volume = 1;
        SceneManager.LoadScene(0);
    }  
}

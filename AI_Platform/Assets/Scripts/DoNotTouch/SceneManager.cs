using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
            return;
        }
        DontDestroyOnLoad(Instance);
        Instance = this;
    }
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
        Instantiate(Resources.Load("Player"), spawnPosition, Quaternion.identity);
    }
  
}

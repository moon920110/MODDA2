using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    #region Variables
    public float p_current_health;
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public CharacterController characterController;
    public GameObject playerCamera;
    public RecoveryBox recoveryBox;
    private Quaternion playerCameraOriginalRotation;
    private float shakeTime;
    private float shakeDuration;    
    //public AudioSource HealthPickup;
    //Weapons
    public GameObject weaponHolder;
    int activeWeaponIndex;
    GameObject activeWeapon;
      
    //UI
    public Text healthNumber;
    public float Score;
    public Text scoreText;
    public float numberofDeath;
    public Text deathText;
    
    public GameObject playerGameObject;
    public PlayerSpawnPoints playerSpawnPoints;
    public CanvasGroup deathPanel;

    //public Transform Destination;
    //public Transform playerLocation;
    #endregion

    void Start()
    {
        playerSpawnPoints = FindObjectOfType<PlayerSpawnPoints>();
        
        playerCameraOriginalRotation = playerCamera.transform.localRotation;
        

        healthNumber.text = p_current_health.ToString();
        weaponSwitch(0);
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //recoveryBox = GameObject.FindWithTag("Recovery").GetComponent<RecoveryBox>();
        
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

        p_current_health = 10 ; //Set to 10 for testing
        //HealthPickup = GetComponent<AudioSource>();
        numberofDeath = 0;

    }
    public void Update()
    {
        /*if (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            cameraShake();
        }
        else if (playerCamera.transform.localRotation != playerCameraOriginalRotation)
        {
            playerCamera.transform.localRotation = playerCameraOriginalRotation;
        }*/
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            weaponSwitch(activeWeaponIndex + 1);
        }        
        scoreText.text = Score.ToString();

        if (Input.GetKeyDown(KeyCode.M))//to test out player spawn
        {
            MoveOnDie();
        }
        /*if (deathPanel.alpha > 0)
        {
            deathPanel.alpha -= Time.deltaTime;
        }*/
    }
    public void Recovered(float recoveryAmount)
    {
        //HealthPickup.Play();
        Debug.Log("First aid taken");
        if (p_current_health <= 80)
        {
            p_current_health = p_current_health + recoveryAmount;
            //Debug.Log("player healed");
            healthNumber.text = p_current_health.ToString();

        }
        else
        {
            p_current_health = 100;
            healthNumber.text = p_current_health.ToString();
        }
    }
    public void Hit(float damage) //Take Damage
    {
        //Debug.Log("player took damage");
        p_current_health -= damage;
        healthNumber.text = p_current_health.ToString();
        

        if (p_current_health <= 0)
        {
            Debug.Log("player died");
            Score = Score - gameManager.deathPenalty;
            scoreText.text = Score.ToString();            
            //p_current_health = 100;            
            
            PlayerDied();
            
        }
        else //int collision with turrent bullet
        {
            shakeTime = 1;
            shakeDuration = 2.6f;
            if (shakeTime < shakeDuration)
            {
                shakeTime += Time.deltaTime;
                cameraShake();
            }
            else if (playerCamera.transform.localRotation != playerCameraOriginalRotation)
            {
                playerCamera.transform.localRotation = playerCameraOriginalRotation;
            }
        }
    }
    public void PlayerDied()
    {
        shakeTime = 0;
        shakeDuration = 0.2f;
        if (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            cameraShake();
            ///p_current_health = 100;
        }
        else if (playerCamera.transform.localRotation != playerCameraOriginalRotation)
        {
            playerCamera.transform.localRotation = playerCameraOriginalRotation;
        }
        //deathPanel.alpha = 1;
        //p_current_health = 100;
        MoveOnDie();
        //StartCoroutine(PlayerSpawn());
        //////p_current_health = 100;
        p_current_health = 100;
        healthNumber.text = p_current_health.ToString();
        numberofDeath = numberofDeath+1;
        deathText.text = numberofDeath.ToString();
        
    }

    private void MoveOnDie()
    {
        
        characterController.enabled = false;
        playerMovement.enabled = false;
        int selectedIndex = Random.Range(0, playerSpawnPoints.SpawnPoints.Count);
        //transform.position = playerSpawnPoints.SpawnPoints[selectedIndex].position;
        Vector3 newPosition = playerSpawnPoints.SpawnPoints[selectedIndex].position;
        newPosition.y = 1.3f;
        transform.position = newPosition;
        characterController.enabled = true;
        playerMovement.enabled = true;        
        Debug.Log("moved!");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terminal")
        {

                Debug.Log("terminal collision");
                gameManager.EndGame();
            
        }
    }
    /*public void PlayerSpawner()
    {        
        playerGameObject.transform.position = PlayerSpawnPointAtStart.transform.position;
        shakeTime = 0;
        shakeDuration = 2f;        
    }*/

    /*IEnumerator PlayerSpawn()
    {
        playerGameObject.GetComponent<PlayerMovement>().enabled = false;
        yield return null;
        //playerGameObject.transform.position = PlayerSpawnPointAtStart.transform.position;
        playerGameObject.GetComponent<PlayerMovement>().enabled = true;
        Debug.Log("moved!");        
        yield return new WaitForSeconds(1);
    }*/

    public void cameraShake()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(Random.Range(-2f, 2f), 0, 0);
    }

    public void weaponSwitch(int weaponIndex)
    {
        int index = 0;
        int amountOfWeapons = weaponHolder.transform.childCount;

        if (weaponIndex > amountOfWeapons - 1)
        {
            weaponIndex = 0;
        }
        foreach (Transform child in weaponHolder.transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
            if (index == weaponIndex)
            {
                child.gameObject.SetActive(true);
                activeWeapon = child.gameObject;
            }
            index++;
        }
        activeWeaponIndex = weaponIndex;
    } 
    }



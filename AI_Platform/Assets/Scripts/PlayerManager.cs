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
    public AudioSource HealthPickup;

    //Weapons
    private GameObject weaponHolder;
    //int activeWeaponIndex;
    //GameObject activeWeapon;
      
    //UI
    public Text healthNumber;
    public float Score;
    public Text scoreText;
    public float numberofDeath;
    public Text deathText;
    public CanvasGroup deathPanel;

    public GameObject playerGameObject;
    public PlayerSpawnPoints playerSpawnPoints;
    private int currentRoom = 0; // Track the current room number
    public LayerMask Ground; // Layer mask for the ground

    private string roomNamePrefix = "Room"; // Define the room name prefix


    #endregion

    void Start()
    {
        playerSpawnPoints = FindObjectOfType<PlayerSpawnPoints>();        
        playerCameraOriginalRotation = playerCamera.transform.localRotation;   
        healthNumber.text = p_current_health.ToString();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        p_current_health = 100 ; 
        HealthPickup = GetComponent<AudioSource>();
        numberofDeath = 0;

    }
    public void Update()
    {
        CheckRoomCollision();

        if (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            cameraShake();
        }
        else if (playerCamera.transform.localRotation != playerCameraOriginalRotation)
        {
            playerCamera.transform.localRotation = playerCameraOriginalRotation;
        }
        
        scoreText.text = Score.ToString();

        if (Input.GetKeyDown(KeyCode.M))//to test out player spawn
        {
            MoveOnDie();
        }
        
    }
    
    public void Recovered()
    {
        //float recoveryAmount = gameManager.recoveryAmount;
        HealthPickup.Play();
        Debug.Log("First aid taken");
        if (p_current_health <= 99)
        {
            p_current_health = p_current_health + gameManager.recoveryAmount;
            Debug.Log("player healed "+ gameManager.recoveryAmount);
            healthNumber.text = p_current_health.ToString();
            if (p_current_health + gameManager.recoveryAmount > 100)
            {
                p_current_health = 100;
                Debug.Log("player health set to 100");
                healthNumber.text = p_current_health.ToString();
            }

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
            PlayerDied();
            
        }
        else //Collision with turrent bullet
        {
            shakeTime = 0.08f;
            shakeDuration = 0.08f;
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
        //Visual effects:
        shakeTime = 0.2f;
        shakeDuration = 0.2f;
        if (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            cameraShake();
        }
        else if (playerCamera.transform.localRotation != playerCameraOriginalRotation)
        {
            playerCamera.transform.localRotation = playerCameraOriginalRotation;
        }
        //Player spawns on a different position after death:
        MoveOnDie();
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
        Vector3 newPosition = playerSpawnPoints.SpawnPoints[selectedIndex].position;
        newPosition.y = 1.3f;
        transform.position = newPosition;
        characterController.enabled = true;
        playerMovement.enabled = true;        
        //Debug.Log("moved!");        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terminal")
        {
                Debug.Log("terminal collision");
                gameManager.EndGame();            
        }
    }
    private void CheckRoomCollision()
    {
        //Checking which room the player is in:
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            string collidedObjectTag = hit.collider.gameObject.tag;

            if (collidedObjectTag.StartsWith("Room"))
            {
                string roomNumber = collidedObjectTag.Substring(4); 
                //Debug.Log("Collided Object Tag: " + collidedObjectTag);
                int roomIndex;
                if (int.TryParse(roomNumber, out roomIndex))
                {
                    if (roomIndex != currentRoom)
                    {
                        currentRoom = roomIndex;
                        Debug.Log("Player entered Room " + currentRoom);
                    }
                    else
                    {
                        //Debug.Log("Player is still in Room " + currentRoom + " but the tile has changed.");
                    }
                }
            }
        }
    }
    //Visual effect for getting hit and dying:
    public void cameraShake()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(Random.Range(-0.5f, 0.5f), 0, 0);
    }
}



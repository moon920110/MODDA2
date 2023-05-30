using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class enemyManager : MonoBehaviour
{
    #region Variables
    private GameObject target;
    private bool targetLocked;
    public GameObject bulletSpawner;
    public GameObject enemyTop;
    public GameObject bullet;
    public float bulletMovementSpeed;
    public float fireTimer = 1f;//
    private bool shootReady;
    
    public GameObject playerObject;
    //public float damage = 50f;    
    public GameManager gameManager;
    public PlayerManager playerManager;    
    
    //public LayerMask canBeShot;
    public LayerMask Ground, playerLayer;
    public NavMeshAgent agent;
    public Transform playerLocation;
    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 6;
    //Attack
    bool alreadyAttacked;
    //States
    public float sightRange = 25, attackRange = 15;
    public bool playerInSightRange, playerInAttackRange, gettingAttack;    
    public float e_current_health; // enemy's updated health
    public int scorePoints = 20;  // When a turret is destroyed, the player gets 20 points   

    //UI
    public Slider slider;
    public AudioSource Laser;
    #endregion


    public void Awake()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        target = playerObject;
        gameManager = GameManager.instance; 
    }

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        e_current_health = gameManager.enemyCurrentHealth;
        Debug.Log(e_current_health);
        slider.maxValue = e_current_health;
        slider.value = e_current_health;
        shootReady = true;
        gettingAttack = false;
        Laser = GetComponent<AudioSource>();
    }
    void Update()
    {
        //detecting player        
        //playerObject = GameObject.FindGameObjectWithTag("Player");
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        slider.gameObject.transform.LookAt(playerObject.transform.position);

        if (!playerInSightRange && !playerInAttackRange)
        {
            if (e_current_health > 0) 
            {
                Patroling();
            }
        }
        if (
            playerInSightRange && !playerInAttackRange)
        {
            targetLocked = true;
            if (targetLocked && playerInSightRange)
            {
                float p_current_health = playerObject.GetComponent<PlayerManager>().p_current_health;
                /*if (p_current_health > 0)
                {
                    Debug.Log(p_current_health);
                    enemyTop.transform.LookAt(target.transform);
                }*/

            }
            if (e_current_health > 0)
            {
                ChasePlayer();
            }
        }
        if (playerInSightRange && playerInAttackRange)
        {            
            /*if (targetLocked)
            {
                enemyTop.transform.LookAt(target.transform);
            }*/
            if (e_current_health > 0 || gettingAttack)
            {
                AttackPlayer();
            }
        }        
    }
    public void Shoot()
    {       
        //RaycastHit t_hit = new RaycastHit(); //don't mind this warning                
        //shootReady = false;
        StartCoroutine(FireRate());
        //shootReady = false;
    }
    IEnumerator FireRate()
    {
        Laser.Play();
        GameObject.Instantiate(bullet.transform, bulletSpawner.transform.position, bulletSpawner.transform.rotation);// Quaternion.identity);
        //Rigidbody _bulletRigidBody = _bullet.GetComponent<Rigidbody>();
        //_bullet.transform.rotation = bulletSpawner.transform.rotation;
        //_bulletRigidBody.AddForce(_bulletRigidBody.transform.forward * bulletMovementSpeed);
        //RaycastHit t_hit = new RaycastHit();
        yield return new WaitForSeconds(2);//(fireTimer);
        shootReady = true;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
            targetLocked = true;
        }
        else
        {
            target = other.gameObject;
            targetLocked = false;
        }
    }

    private void AttackPlayer()
    {        
        if (e_current_health > 0)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(playerLocation);
        }
        //if (!alreadyAttacked)
        //{
            
            if (targetLocked)
            {
                enemyTop.transform.LookAt(playerObject.transform);
                enemyTop.transform.Rotate(4, 0, 0);                
                if (shootReady)
                { Shoot(); }
            }
        //}
    }
    public void Hit(float damage) //When the enemy Takes Damage
    {
        //Debug.Log("enemy took damage");
        gettingAttack = true;
        e_current_health -= damage;        
        slider.value = e_current_health;
        if (e_current_health <= 0)
        {
            Debug.Log("enemy down");
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<enemyManager>());
            Destroy(GetComponent<BoxCollider>());
            Destroy(this.gameObject, 0.0001f);
            //numberofDestroyed = numberofDestroyed + 1;
            //Debug.Log("Number of destroyed on enemy script: " + numberofDestroyed);
        }        
    }
    public void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
        {
            walkPointSet = true;//
        }
    }
    public void ChasePlayer()
    {
        if (playerInSightRange) //&& playerInAttackRange
        {
            agent.SetDestination(playerLocation.position);
        }
    }   
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }
}

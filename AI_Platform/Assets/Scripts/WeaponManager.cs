using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public GameObject playerCam;
    public GameObject WeaponHolder;
    public int range = 100;
    public int damage = 25;
    //public Animator playerAnimator;
    public ParticleSystem muzzleFlash;
    //public GameObject hitParticles;
    //public AudioClip gunshot;
    public WeaponSway weaponSway;
    float swaySensitivity;
    public GameObject playerBullet;
    
    public GameObject p_bulletSpawner;
    public GameObject crosshair;
    public GameObject nonTargetHitParticles;
    public float firerate = 10;
    float firerateTimer = 0;
    public float numberofDestroyed;

    //public bool isAutomatic;
    //public string weaponType;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public AudioSource MachineGun;
    //public bool isAiming = false;
    //public Transform t_state_Aiming;
    //public Transform t_state_notAiming;

    #endregion
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        swaySensitivity = weaponSway.swaySensitivity;
        MachineGun = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        numberofDestroyed = 0;
    }

    private void OnEnable()
    {
        //playerAnimator.SetTrigger(weaponType);
    }

    /*private void OnDisable()
    {
        //playerAnimator.SetBool("isReloading", false);
        //isReloading = false;
        //Debug.Log("Reload Interupted");
    }*/


    void Update()
    {
        /*if (playerAnimator.GetBool("isShooting"))
        {
            playerAnimator.SetBool("isShooting", false);

        }*/
        if (firerateTimer > 0)
        {
            firerateTimer = firerateTimer - Time.deltaTime;
        }
        if (Input.GetButton("Fire1") && firerateTimer <= 0)// && isAutomatic)
        {
            //isAiming = true;
            //Aim(isAiming);
            Shoot();
            firerateTimer = 1 / firerate;
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            //Aim(true);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            /*if (playerAnimator.GetBool("isAiming"))
            {
                playerAnimator.SetBool("isAiming", false);
            }*/
            weaponSway.swaySensitivity = swaySensitivity;
            crosshair.SetActive(true);
        }
        else if (Input.GetButtonUp("Fire1"))
    {
        //isAiming = false;
        //Aim(isAiming);
    }
        
    }

    void Shoot()
    {
        //muzzleFlash.Play();        
        MachineGun.Play();
        GameObject.Instantiate(playerBullet.transform, p_bulletSpawner.transform.position, p_bulletSpawner.transform.rotation);
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range))
        {

            enemyManager enemyManager = hit.transform.GetComponent<enemyManager>();
            if (enemyManager != null)
            {
                enemyManager.Hit(damage);
                if (enemyManager.e_current_health <= 0)
                {
                    playerManager.Score += enemyManager.scorePoints; //adding score to the player                    
                    numberofDestroyed = numberofDestroyed + 1;
                    Debug.Log("Number of destroyed on weapon script: " + numberofDestroyed);

                }
                
                //Debug.Log("enemy hit");
            }
            else
            {
                
                //Debug.Log("non-enemy hit");
            }
        }
    }

    /*void Aim(bool isAiming)
    {
        Transform t_state_Aiming = WeaponHolder.transform.Find("States/Aiming");
        Transform t_state_notAiming = WeaponHolder.transform.Find("States/notAiming");

        if (isAiming)
        {
            DisableImmediateChildren(t_state_notAiming);
            EnableImmediateChildren(t_state_Aiming);
        }
        else
        {
            DisableImmediateChildren(t_state_Aiming);
            EnableImmediateChildren(t_state_notAiming);
        }
    }

    void DisableImmediateChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
    }

    void EnableImmediateChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(true);
        }
    }*/
    /*public bool Aim(bool p_isAiming)
    {

            /isAiming = p_isAiming;
            Transform t_anchor = Weapon.transform.Find("Root");
            Transform t_state_Aiming = Weapon.transform.Find("States/Aiming");
            Transform t_state_notAiming = Weapon.transform.Find("States/notAimingHip");

            if (p_isAiming)
            {
                //aim
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_Aiming.position, Time.deltaTime);
            }
            else
            {
                //hip
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_notAiming.position, Time.deltaTime);
            }

            return p_isAiming;
        
        //playerAnimator.SetBool("isAiming", true);
        //weaponSway.swaySensitivity = swaySensitivity / 3;
        //crosshair.SetActive(false);*/
}
    
  


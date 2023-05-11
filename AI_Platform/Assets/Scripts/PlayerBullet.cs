using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    #region Variables
    public GameObject playerBullet;
    public float p_bulletSpeed = 100;
    //public GameObject playersTarget;
    public GameObject Enemy;
    
    //public float turretFirerate = 100;
    //float turretFirerateTimer = 0;
    //public bool isTurretAutomatic;
    #endregion

    private void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("enemy");
    }
    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * p_bulletSpeed);
        //transform.position += transform.forward * p_bulletSpeed * Time.deltaTime;
        /*if (turretFirerateTimer > 0)
        {
            turretFirerateTimer = turretFirerateTimer - Time.deltaTime;
        }
        if (turretFirerateTimer <= 0) // && isTurretAutomatic)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            turretFirerateTimer = 1 / turretFirerate;
        }
        if (turretFirerateTimer <= 0)// && !isTurretAutomatic)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            turretFirerateTimer = 1 / turretFirerate;
            
        }*/
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            //Debug.Log("enemy hit");
            Enemy = other.gameObject;
            Destroy(playerBullet, 0.000001f);
           
        }
        else if (other.tag == "Wall")

        {
            //Debug.Log("Wall hit");
            Destroy(playerBullet, 0.00001f);
        }


    }
}

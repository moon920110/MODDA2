using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.UI;

public class enemyBullet : MonoBehaviour
{
    #region Variables
    public float movementSpeed;
    private GameObject target;
    public GameObject bullet;
    public GameObject playerObject;    
    public float damage;
    //public float enemyFirerate = 100;
    //float enemyFirerateTimer = 0;
    //public bool isenemyAutomatic;
    #endregion

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);

        /*if (enemyFirerateTimer > 0)
        {
            enemyFirerateTimer = enemyFirerateTimer - Time.deltaTime;
        }
        if (enemyFirerateTimer <= 0) // && isenemyAutomatic)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            enemyFirerateTimer = 1 / enemyFirerate;
        }
        if (enemyFirerateTimer <= 0)// && !isenemyAutomatic)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            enemyFirerateTimer = 1 / enemyFirerate;
            
        }*/
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
            Destroy(bullet, 0.000001f);
            playerObject.GetComponent<PlayerManager>().Hit(damage);            
        }
        else if (other.tag == "Wall")
        {
            Destroy(bullet,0.0001f);
        }
    }
}

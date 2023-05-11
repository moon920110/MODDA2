using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoints : MonoBehaviour
{
    public List<Transform> SpawnPoints = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SpawnPoints.Add(transform.GetChild(i));
        }
    }
}

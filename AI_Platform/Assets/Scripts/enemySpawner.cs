using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemySpawner : MonoBehaviour
{

    public int[] initialEnemiesByRoom = { 2, 3, 3, 2, 2, 2, 1, 4, 5 };
    private int[] currentEnemiesByRoom;
    private GameObject[] tilesByRoom;
    private GameObject[] enemies;
    private GameObject enemyPrefab;
    private bool[] roomCleared;
    private float spawnDelay = 5f;

    private void Start()
    {
        enemyPrefab = (GameObject)Resources.Load("Enemy");
        currentEnemiesByRoom = initialEnemiesByRoom;
        tilesByRoom = new GameObject[9];
        enemies = new GameObject[currentEnemiesByRoom.Length];
        roomCleared = new bool[9];

        for (int i = 1; i <= 9; i++)
        {
            tilesByRoom[i - 1] = GameObject.FindGameObjectsWithTag("Room" + i)[0];
        }

        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        currentEnemiesByRoom = new int[initialEnemiesByRoom.Length];

        for (int i = 0; i < initialEnemiesByRoom.Length; i++)
        {
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Room" + (i + 1));

            int numEnemies = initialEnemiesByRoom[i];

            for (int j = 0; j < numEnemies; j++)
            {
                Vector3 spawnPos = GetRandomNavmeshPosition(tiles);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                currentEnemiesByRoom[i]++;
            }
        }
    }
    private Vector3 GetRandomNavmeshPosition(GameObject[] tiles)
    {
        // Select a random tile
        int tileIndex = Random.Range(0, tiles.Length);
        GameObject tile = tiles[tileIndex];

        // Get a random position on the tile's navmesh
        NavMeshHit hit;
        NavMesh.SamplePosition(tile.transform.position, out hit, 5.0f, NavMesh.AllAreas);
        return hit.position;
    }
    private void SpawnEnemiesByRoom(int roomIndex, int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject enemy = Instantiate(Resources.Load("Enemy")) as GameObject;
            enemies[roomIndex * numEnemies + i] = enemy;

            Transform randomTile = tilesByRoom[roomIndex].transform.GetChild(Random.Range(0, 6));
            NavMeshHit hit;
            NavMesh.SamplePosition(randomTile.position, out hit, 2f, NavMesh.AllAreas);
            enemy.transform.position = hit.position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentEnemiesByRoom.Length; i++)
        {
            if (roomCleared[i]) continue;

            if (currentEnemiesByRoom[i] == 0)
            {
                roomCleared[i] = true;
                StartCoroutine(SpawnEnemiesAfterDelay(i));
            }
        }
    }

    private IEnumerator SpawnEnemiesAfterDelay(int roomIndex)
    {
        yield return new WaitForSeconds(spawnDelay);
        currentEnemiesByRoom[roomIndex] = initialEnemiesByRoom[roomIndex];
        SpawnEnemiesByRoom(roomIndex, currentEnemiesByRoom[roomIndex]);
        roomCleared[roomIndex] = false;
    }
}
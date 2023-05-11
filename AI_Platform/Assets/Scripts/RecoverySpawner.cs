using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RecoverySpawner : MonoBehaviour
{

    public int[] initialAidsByRoom = { 2, 3, 3, 2, 2, 2, 1, 4, 5 };
    private int[] currentAidsByRoom;
    private GameObject[] tilesByRoom;
    private GameObject[] firstaids;
    private GameObject firstaidPrefab;
    private bool[] roomCleared;
    private float spawnDelay = 5f;

    private void Start()
    {
        firstaidPrefab = (GameObject)Resources.Load("FirstAid");
        currentAidsByRoom = initialAidsByRoom;
        tilesByRoom = new GameObject[9];
        firstaids = new GameObject[currentAidsByRoom.Length];
        roomCleared = new bool[9];

        for (int i = 1; i <= 9; i++)
        {
            tilesByRoom[i - 1] = GameObject.FindGameObjectsWithTag("Room" + i)[0];
        }

        SpawnInitialAids();
    }

    private void SpawnInitialAids()
    {
        currentAidsByRoom = new int[initialAidsByRoom.Length];

        for (int i = 0; i < initialAidsByRoom.Length; i++)
        {
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Room" + (i + 1));

            int numAids = initialAidsByRoom[i];

            for (int j = 0; j < numAids; j++)
            {
                Vector3 spawnPos = GetRandomNavmeshPosition(tiles);
                Instantiate(firstaidPrefab, spawnPos, Quaternion.identity);
                currentAidsByRoom[i]++;
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
    private void SpawnAidsByRoom(int roomIndex, int numAids)
    {
        for (int i = 0; i < numAids; i++)
        {
            GameObject firstaid = Instantiate(Resources.Load("FirstAid")) as GameObject;
            firstaids[roomIndex * numAids + i] = firstaid;

            Transform randomTile = tilesByRoom[roomIndex].transform.GetChild(Random.Range(0, 6));
            NavMeshHit hit;
            NavMesh.SamplePosition(randomTile.position, out hit, 2f, NavMesh.AllAreas);
            firstaid.transform.position = hit.position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentAidsByRoom.Length; i++)
        {
            if (roomCleared[i]) continue;

            if (currentAidsByRoom[i] == 0)
            {
                roomCleared[i] = true;
                StartCoroutine(SpawnAidsAfterDelay(i));
            }
        }
    }

    private IEnumerator SpawnAidsAfterDelay(int roomIndex)
    {
        yield return new WaitForSeconds(spawnDelay);
        currentAidsByRoom[roomIndex] = initialAidsByRoom[roomIndex];
        SpawnAidsByRoom(roomIndex, currentAidsByRoom[roomIndex]);
        roomCleared[roomIndex] = false;
    }
}
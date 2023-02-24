using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] prefabSpawn;

    public CinemachineVirtualCamera cvCam;

    [Header("Random time in between Spawn")]
    public float minTimeSpawn = 0;
    public float maxTimeSpawn = 2;

    [Header("Max spawns in game")]
    public int maxSpawn;

    [HideInInspector]
    public int ammountSpawn;

    [Header("apenas teste depois apaga")]
    public int minDistanceRespawns;
    public int maxDistanceRespawns;

    public List<Transform> allSpawnsPositions;
    public List<Transform> selectSpawns;

    private void Start()
    {
        StartCoroutine("LoopSpawns");
    }

    void Update()
    {
        
    }

    IEnumerator LoopSpawns()
    {
        selectSpawns = new List<Transform>();

        foreach (Transform positionSpawn in allSpawnsPositions) // Verefy Spawns in range and Added selectedSpawns Lis
        {
            float distance = Vector2.Distance(transform.position, cvCam.transform.position);

            if (distance > minDistanceRespawns && distance < maxDistanceRespawns)
            {
                selectSpawns.Add(positionSpawn);
            }
        }

        float timerRandom = Random.Range(minTimeSpawn, maxTimeSpawn);

        float selectRandomSpawn = Random.Range(0, selectSpawns.Count - 0.1f);

        float randomPrefab = Random.Range(0, prefabSpawn.Length - 0.1f);

        yield return new WaitForSeconds(timerRandom);

        if(ammountSpawn < maxSpawn)
        {
            GameObject.Instantiate(prefabSpawn[(int)randomPrefab], selectSpawns[(int)selectRandomSpawn].position, selectSpawns[(int)selectRandomSpawn].rotation);
            ammountSpawn++;
        }

        StartCoroutine("LoopSpawns");
    }
}

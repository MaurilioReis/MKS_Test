using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawns : MonoBehaviour
{

    [Header("CONFIG SPAWNS")]

    [Space(10)]
    [Header("Add Spawns")]
    public GameObject[] objectsSpawns;
    int selectSpawn = 0;

    [Header("Pos start spawns")]
    public Transform posRotSpawn;

    [Header("Random distance between spawns")]
    public Vector2 minDistanceBetweenSpawns = new Vector2(0, 0);
    public Vector2 maxDistanceBetweenSpawns = new Vector2(0, 0);

    [Header("If randomize rotate")]
    public bool randomizeRot;
    [Header("Value random rotate")]
    [Range(0,360)]
    public float randomRot;

    [Header("Minimun and Maximum scale")]
    public Vector2 minScale = new Vector2(1, 1);
    public Vector2 maxScale = new Vector2(1, 1);

    [Header("Minimun and Maximum amount of spawns")]
    public int minSpawns;
    public int maxSpawns;

    [Header("Force to move spawns")]
    public float addForceSpawn = 0;

    [Range(0, 360)]
    public float randomDirForce;

    [Header("Time to start spawns")]
    public float timeStartSpawn = 0.1f;

    [Header("Timer to setween spawns")]
    public float timeBetweenSpawns = 0.1f;

    //[Header("TRIGGER TO START SPAWNS")]

    //public bool collisionEnter;
    //public bool collisionExit;
    //public bool collisionTriggerEnter;
    //public bool collisionTriggerEXit;

    private void Start()
    {
        StartCoroutine("StartSpawn");
    }

    IEnumerator StartSpawn()
    {
        yield return new WaitForSecondsRealtime(timeStartSpawn); // start

        int amountSpawns = Random.Range(minSpawns, maxSpawns);

        for (int nSpawn = 0; nSpawn < amountSpawns; nSpawn++)
        {
            if (selectSpawn < objectsSpawns.Length)
            {
                Vector2 randomDistancePosition = new Vector2(Random.Range(minDistanceBetweenSpawns.x, maxDistanceBetweenSpawns.x), Random.Range(minDistanceBetweenSpawns.y, maxDistanceBetweenSpawns.y));
                Vector2 regulePositionSpawn = new Vector2(posRotSpawn.position.x + randomDistancePosition.x, posRotSpawn.position.y + randomDistancePosition.y);

                if (randomizeRot == true)
                posRotSpawn.transform.Rotate(0.0f, 0.0f, Random.Range(0, randomRot), Space.Self);

                GameObject instance = Instantiate(objectsSpawns[selectSpawn], regulePositionSpawn, posRotSpawn.rotation) as GameObject;

                Vector2 reguleScale = new Vector2(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y));
                instance.transform.GetChild(0).localScale = reguleScale;

                Rigidbody2D rbSpawns = instance.GetComponentInChildren<Rigidbody2D>();

                transform.Rotate(0.0f, 0.0f, transform.rotation.z + Random.Range(-randomDirForce, randomDirForce), Space.Self);

                if (rbSpawns != null)
                rbSpawns.AddForce(transform.right * addForceSpawn, ForceMode2D.Impulse);

                selectSpawn++;

                yield return new WaitForSecondsRealtime(timeBetweenSpawns);
            }
            else
            {
                selectSpawn = 0;
            }
        }

        Destroy(gameObject);
    }
}

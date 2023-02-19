using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submerge : MonoBehaviour
{
    [Header(" --------------------------------- CONFIG PARAMETERS")]
    [Space(15)]
    [Header("min and max time to start submerge")]
    public float minTimeToSubmerge;
    public float maxTimeToSubmerge;
    float timeSubmerge;

    [Header("time to start submerge SEABED")]
    public float timeSubmergeSeabed = 3.5f;

    [Space(10)]
    [Header ("percentage chance of increase time to start submerge")]
    [Range(0, 100)]
    public int percentChanceIncreaseTime;
    public float timeIncrease;

    [Space(10)]
    [Header("Speed submerge")]
    [Range(0.1f, 3)]
    public float speed = 1;

    [Space(15)]
    [Header(" --------------------------------- START SCRIPT")]
    [Space(15)]

    [Header("Colliders to desactive in start")]
    public Collider2D[] collidersDesactive;

    [Space(10)]
    [Header("GameObjects to desactive in start")]
    public GameObject[] gameObjectDesactive;

    [Space(10)]
    [Header("GameObjects to destroy in start")]
    public GameObject[] gameObjectDestroy;

    [Space(15)]
    [Header("  --------------------------------- START SUBMERGE")]
    [Space(15)]

    [Header("Sprite to change the layer in start submerge")]
    public SpriteRenderer[] spritesSubmerge;
    [Header("Colliders to desactive in start submerge")]
    public Collider2D[] colliders;
    [Header("Objects active in start submerge")]
    public GameObject[] fx;
    [Header("Objects instantiate in start submerge")]
    public GameObject[] instantiateObjects;
    [Header("Objects desactive in start submerge")]
    public GameObject[] desactiveInStart;

    bool inSubmerge;

    void Start()
    {
        if (collidersDesactive.Length != 0)
            foreach (Collider2D col in collidersDesactive)
        {
            col.enabled = false;
        }

        if(gameObjectDesactive.Length != 0)
        foreach (GameObject go in gameObjectDesactive)
        {
            if (go != null)
            {
                go.transform.parent = null;

                ParticleSystem ps = go.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Stop(true);
                }
            }
        }

        if (gameObjectDestroy.Length != 0)
            foreach (GameObject go in gameObjectDestroy)
            {
                Destroy(go);
            }

        int dice = Random.Range(0, 100);
        if (dice < percentChanceIncreaseTime)
        {
            minTimeToSubmerge = minTimeToSubmerge + timeIncrease;
            maxTimeToSubmerge = maxTimeToSubmerge + timeIncrease;
        }

        timeSubmerge = Random.Range(minTimeToSubmerge, maxTimeToSubmerge);

        StartCoroutine("StartSubmerge");
    }

    IEnumerator StartSubmerge()
    {
        yield return new WaitForSecondsRealtime(timeSubmerge);

        if (spritesSubmerge.Length != 0)
            foreach (SpriteRenderer sr in spritesSubmerge)
        {
            sr.sortingLayerName = "Submerse";
        }

        if (colliders.Length != 0)
            foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (fx.Length != 0)
            foreach (GameObject go in fx)
        {
            go.SetActive(true);
        }
        
        if (instantiateObjects.Length != 0)
            foreach (GameObject go in instantiateObjects)
        {
            GameObject instance = Instantiate(go, transform.position, transform.rotation) as GameObject;
        }

        if (desactiveInStart.Length != 0)
            foreach (GameObject go in desactiveInStart)
        {
            if (go != null)
            {
                go.transform.parent = null;

                ParticleSystem ps = go.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Stop(true);
                }
            }  
        }

        inSubmerge = true;

        yield return new WaitForSecondsRealtime(timeSubmergeSeabed);

        if (spritesSubmerge.Length != 0)
            foreach (SpriteRenderer sr in spritesSubmerge)
            {
                sr.sortingLayerName = "Seabed";
            }

    }


    private void FixedUpdate()
    {
        if (inSubmerge)
        {
            transform.localScale -= new Vector3(Time.deltaTime/10 * speed, Time.deltaTime / 10 * speed, Time.deltaTime / 10 * speed);

            if (transform.localScale.x <= 0)
            {
                foreach (GameObject go in fx)
                {
                    go.transform.parent = null;

                    ParticleSystem ps = go.GetComponent<ParticleSystem>();

                    if (ps != null)
                    {
                        ps.Stop(true);
                    }
                }

                Destroy(gameObject);
            }
        }
    }

}


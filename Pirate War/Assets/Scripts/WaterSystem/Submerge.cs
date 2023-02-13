using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submerge : MonoBehaviour
{

    [Header("min and max time to start submerge")]
    public float minTimeToSubmerge;
    public float maxTimeToSubmerge;
    float timeSubmerge;

    [Header ("percentage chance of increase time to start submerge")]
    [Range(0, 100)]
    public int percentChanceIncreaseTime;
    public float timeIncrease;

    [Header("Sprite to change the layer")]
    public SpriteRenderer[] spritesSubmerge;
    [Header("Colliders to desactive")]
    public Collider2D[] colliders;
    [Header("Objects active in start submerge")]
    public GameObject[] fx;
    [Header("Objects desactive in start submerge")]
    public GameObject[] desactiveInStart;

    bool inSubmerge;

    void Start()
    {
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

        foreach (SpriteRenderer sr in spritesSubmerge)
        {
            sr.sortingLayerName = "Submerse";
        }

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        foreach (GameObject go in fx)
        {
            go.SetActive(true);
        }

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
    }


    private void FixedUpdate()
    {
        if (inSubmerge)
        {
            transform.localScale -= new Vector3(Time.deltaTime/10, Time.deltaTime / 10, Time.deltaTime / 10);

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


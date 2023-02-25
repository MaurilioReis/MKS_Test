using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParameters : MonoBehaviour
{
    [Header("WARNING: if any variation float is set to zero, the respective function will not be activated.")]
    [InspectorName("test")]

    [Space(10)]

    [Header("DAMAGE")]
    [Space (5)]
    public float dmg;

    [Space(10)]

    Rigidbody2D rb;
    [Header("CONTROLLER VELOCITYS")]

    [Space(5)]
    [Header("set start speed")]
    public float startSpeed = 1;

    [Space(5)]
    [Header("set speed on distance")]
    public float distanceSpeed = 0;
    public Transform distanceDirection;
    public float distanceAltereSpeed;

    [Space(5)]
    [Header("Set Angular Velocity")]
    [Range(-1000, 1000)]
    public float setAngularVelocity = 0;

    [Space(5)]
    [Header("Random Angular Velocity")]
    [Range(-1000, 1000)]
    public float minAngularVelocity = 0;
    [Range(-1000, 1000)]
    public float maxAngularVelocity = 0;

    [Space(10)]

    [Header("SET INSTANTIATE SPAWNS")]

    [Space(5)]
    [Header("Spawns in start")]
    public GameObject[] spawnsStart;

    [Space(5)]
    [Header("Spawns in new rb velocity")]
    public GameObject[] spawnNewVelocity;

    [Space(5)]
    [Header("Spawns in timer")]
    public GameObject[] spawnsInTimer;
    public float timerSpawns;

    [Space(5)]
    [Header("Spawns impact")]
    public GameObject[] spawnsImpactWoods;

    [Space(5)]
    [Header("Spawn at end of max distance")]
    public GameObject[] spawnsEndDistance;

    [Space(10)]

    [Header("drop and desactive GameObject")]

    [Space(5)]
    [Header("Parent Null and stop particle in GameObject")]
    public GameObject[] parentNull;

    [Space(10)]

    [Header("CONTROLLER COLLIDER")]

    [Space(5)]
    [Header("Ignore collision on distance")]
    public Collider2D[] colliderDisabled;
    public float distanceActiveColliders;

    [Space(10)]

    [Header("SUBMERGE")]

    [Space(5)]
    [Header("active or desactive submerge system")]
    public bool submerge = false;
    Submerge sbmg;

    [HideInInspector]public Vector2 startPositionOrigin;
    [HideInInspector]public Vector2 directionOrigin;

    float distance;

    [HideInInspector] public float maxDistance;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        foreach (Collider2D col in colliderDisabled)
        {
            col.enabled = false;
        }

        if (distanceDirection != null)
        {
            directionOrigin = transform.right;
        }

        rb.velocity = directionOrigin * startSpeed;

        if (setAngularVelocity != 0)
        {
            if (rb != null)
                rb.angularVelocity = setAngularVelocity;
        }
        else if (minAngularVelocity != 0 || maxAngularVelocity != 0)
        {
            float finalAngularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);

            if(rb != null)
                rb.angularVelocity = finalAngularVelocity;
        }

        foreach (GameObject spawn in spawnsStart)
        {
            GameObject intance = Instantiate(spawn, gameObject.transform.position, gameObject.transform.rotation);
        }

        if (timerSpawns > 0)
        {
            StartCoroutine("TimerSpawns");
        }

        if (submerge)
        {
            sbmg = gameObject.GetComponent<Submerge>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Dead" || collision.gameObject.tag == "Fire")
        {
            GameObject gameObjectCol = collision.gameObject;
            StartColision(gameObjectCol);
        }
    }

    public void StartColision(GameObject gameObjectCol)
    {
        AttributesBase scriptAttributes = gameObjectCol.gameObject.GetComponent<AttributesBase>();
        if (scriptAttributes != null)
            scriptAttributes.applyDmg(dmg);

        foreach (GameObject spawn in spawnsImpactWoods)
        {
            GameObject intance = Instantiate(spawn, gameObject.transform.position, gameObject.transform.rotation);
        }

        foreach (GameObject go in parentNull)
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

    IEnumerator TimerSpawns()
    {
        yield return new WaitForSecondsRealtime(timerSpawns);

        foreach (GameObject spawn in spawnsInTimer)
        {
            GameObject intance = Instantiate(spawn, gameObject.transform.position, gameObject.transform.rotation);
            // Submerse
        }
    }

    private void Update()
    {
        distance = Vector2.Distance(startPositionOrigin, transform.position);

        if (maxDistance > 0 && distance >= maxDistance)
        {
            foreach (GameObject spawn in spawnsEndDistance)
            {
                GameObject intance = Instantiate(spawn, gameObject.transform.position, gameObject.transform.rotation);
            }

            foreach (GameObject go in parentNull)
            {
                go.transform.parent = null;

                ParticleSystem ps = go.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Stop(true);
                }
            }

            maxDistance = 0;

            if (sbmg != null)
            {
                rb.velocity = Vector2.zero;
                sbmg.enabled = true;
                Destroy(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (distanceAltereSpeed > 0 && distance >= distanceAltereSpeed)
        {
            rb.velocity = directionOrigin * distanceSpeed;

            foreach (GameObject spawn in spawnNewVelocity)
            {
                GameObject intance = Instantiate(spawn, gameObject.transform.position, gameObject.transform.rotation);
            }

            distanceAltereSpeed = 0;
        }

        if (distanceActiveColliders > 0 && distance >= distanceActiveColliders)
        {
            foreach (Collider2D col in colliderDisabled)
            {
                col.enabled = true;
            }
        }     
    }

}

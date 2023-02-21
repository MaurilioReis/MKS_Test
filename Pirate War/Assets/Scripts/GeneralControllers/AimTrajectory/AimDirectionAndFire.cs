using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDirectionAndFire : MonoBehaviour
{
    [HideInInspector] public bool inAtack = false;

    [HideInInspector] public bool triggerFire;
    [HideInInspector] public int sideAim;
    [HideInInspector] public int lockAim;

    [Header("attributes based on:")]
    public AttributesBase basedAttributes;

    [Space(10)]
    [Header("Animations trajectory")]
    public Animator trajectorysAnim;

    [Space(10)]
    [Header("Ship sprites to be transparent.")]
    public SpriteRenderer[] spritesShip;


    [Space(10)]
    [Header("Sides trigger")]
    public GameObject[] triggersSide;
    public GameObject fxPadlock;

    [Space(10)]
    [Header("SPAWNS CONFIG")]
    [Header("Element 0 Canceled / 1 front / 2 left / 3 right / 4 back / 5 neutral")]


    [Space(10)]
    public ArmamentParameters[] weapons;

    [HideInInspector]
    public bool inCooldown = false;

    [Space(10)]
    Transform[] spawnOrigins;
    public float timeToSpawn = 0.1f;
    public float timeBetweenSpawns = 0.1f;

    int uID;


    private void Start()
    {
        uID = gameObject.GetInstanceID(); // get number instance

        foreach (GameObject go in triggersSide) 
        {
            go.name = go.name + uID; // set number instance
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            
        if (collision.name == "TriggerFront" + uID)    
        {
            sideAim = 1;
        }
  
        if (collision.name == "TriggerLeft" + uID)  
        { 
            sideAim = 2;
        }

        if (collision.name == "TriggerRight" + uID) 
        {      
            sideAim = 3;
        }
 
        if (collision.name == "TriggerBack" + uID)  
        {     
            sideAim = 4;
        }
    }

    private void Update()
    {

        if (trajectorysAnim != null)
        {
            if (triggerFire == false)
            {
                trajectorysAnim.SetInteger("SideAim", lockAim);

                foreach (SpriteRenderer sp in spritesShip)
                {
                    Color c = sp.color;
                    c.a = 1;
                    sp.color = c;
                }
            }
            else
            {
                trajectorysAnim.SetInteger("SideAim", sideAim);

                foreach (SpriteRenderer sp in spritesShip)
                {
                    Color c = sp.color;
                    c.a = 0.3f;
                    sp.color = c;
                }
            }
        }
    }

    public IEnumerator Fire()
    {
        if (basedAttributes.timerCooldown[sideAim] > 0 && inCooldown == false)
        {
            basedAttributes.timerCooldown[sideAim] = 0;

            inAtack = true;

            int registerSide = sideAim;
            spawnOrigins = weapons[registerSide].originsPositionsSpawns;
            weapons[registerSide].inAtack = true;

            yield return new WaitForSecondsRealtime(timeToSpawn);

            for (int nSpawn = 0; nSpawn < spawnOrigins.Length; nSpawn++)
            {
                if (spawnOrigins[nSpawn] != null && spawnOrigins[nSpawn].gameObject.activeSelf)
                {
                    GameObject fire = Instantiate(weapons[registerSide].ammunition, spawnOrigins[nSpawn].position, spawnOrigins[nSpawn].rotation) as GameObject;

                    FireParameters scriptFire = fire.GetComponent<FireParameters>();
                    scriptFire.maxDistance = weapons[registerSide].GetComponent<ArmamentParameters>().maxDistance;
                    scriptFire.startPositionOrigin = spawnOrigins[nSpawn].transform.position;
                    scriptFire.directionOrigin = spawnOrigins[nSpawn].right;

                    // audio

                    yield return new WaitForSecondsRealtime(timeBetweenSpawns);
                }
            }
            weapons[registerSide].inAtack = false;
            inAtack = false;
        }
        else
        {
            Instantiate(fxPadlock, transform.position, transform.rotation);
        }


    }
}

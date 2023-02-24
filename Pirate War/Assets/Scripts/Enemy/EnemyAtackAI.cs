using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtackAI : MonoBehaviour
{
    public EnemyMoveAi enemyMove;
    public AimDirectionAndFire scriptAimAndFire;
    public AttributesBase scriptParametersBase;

    public float timeMinAtackFrequency = 0.5f;
    public float timeMaxAtackFrequency = 1f;

    bool fire;
    void Start()
    {
    }

   
    void Update()
    {
        if (enemyMove.freeToAtack && enemyMove.distance < 3)
        {
            scriptAimAndFire.triggerFire = true;

            if (scriptAimAndFire.triggerFire == true && fire == false)
            {
                fire = true;
                StartCoroutine("randomAtackFrequency");
            }

            scriptAimAndFire.lockAim = 0;
        }
        else
        {
            scriptAimAndFire.triggerFire = false;
        }
    }

    IEnumerator randomAtackFrequency()
    {
        float timerRandom = Random.Range(timeMinAtackFrequency, timeMaxAtackFrequency);
        yield return new WaitForSeconds(timerRandom);
        scriptAimAndFire.StartCoroutine("Fire");
        scriptAimAndFire.triggerFire = false;
        fire = false;
    }
}

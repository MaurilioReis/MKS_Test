using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtackAI : MonoBehaviour
{
    public EnemyMoveAi enemyMove;
    public AimDirectionAndFire scriptAimAndFire;
    public AttributesBase scriptParametersBase;

    [Space (10)]

    [Header ("select the minimum and maximum fire frequency.")]
    [Range(0.1f, 10)]
    public float timeMinAtackFrequency = 0.5f;
    [Range(0.1f, 10)]
    public float timeMaxAtackFrequency = 1f;

    bool fire;

    [Space(10)]
    [Header("Minimum distance to shoot or explode.")]
    [Range(0,15)]
    public float minDistance;

    void Update()
    {
        if (enemyMove.typeBehavior == 1) // if Shooter
        {
            if (timeMinAtackFrequency > timeMaxAtackFrequency)
            {
                timeMaxAtackFrequency = timeMinAtackFrequency;
            }

            if (enemyMove.freeToAtack && enemyMove.distance < minDistance)
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
        else // ------------------------- if kamikaze exploder
        {
            if (enemyMove.freeToAtack && enemyMove.distance < minDistance)
            {
                scriptParametersBase.applyDmg(scriptParametersBase.maxLife);
            }
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

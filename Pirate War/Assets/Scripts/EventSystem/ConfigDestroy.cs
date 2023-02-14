using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDestroy : MonoBehaviour
{
    public GameObject objectDestroy;

    [Space (10)]
    public bool destroyImmediately;

    [Space(10)]
    public float timerDestroy;

    private void Start()
    {
        if(timerDestroy > 0)
        {
            StartCoroutine("TimerDestroy");
        }
    }

    void Update()
    {
        if (destroyImmediately) 
        {
            if(objectDestroy != null)
            {
                Destroy(objectDestroy);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator TimerDestroy()
    {
        yield return new WaitForSeconds(timerDestroy);

        if (objectDestroy != null)
        {
            Destroy(objectDestroy);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

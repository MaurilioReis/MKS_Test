using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [Header("Timer level")]

    [Range(5, 9999)]
    public float timer = 1.30f;
    public TMP_Text textTimer;

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            textTimer.text = "" + (int)timer;
        }
        else
        {
            timer = 0;
            textTimer.text = "END GAME";
        }

    }
}

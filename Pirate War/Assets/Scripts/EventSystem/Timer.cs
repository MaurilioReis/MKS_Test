using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [Header("Timer level")]

    public float timer;
    public TMP_Text textTimer;
    public Animator anim;

    bool lastSeconds = false;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            string minutes = Mathf.Floor(timer / 59).ToString("00");
            string seconds = (timer % 59).ToString("00");

            textTimer.text = "" + (string.Format("{0}:{1}", minutes, seconds));

            if (seconds == "30" || seconds == "00")
            {
                textTimer.color = new Vector4(+ Time.deltaTime, + Time.deltaTime,- Time.deltaTime, 255);
                anim.SetBool("Yellow", true);
            }
            else
            {
                textTimer.color = new Vector4(+Time.deltaTime, +Time.deltaTime, +Time.deltaTime, 255);
                anim.SetBool("Yellow", false);
            }

            if (timer <= 5.5f && lastSeconds == false)
            {
                StartCoroutine("LastSeconds");
                lastSeconds = true;
            }
        }
        else if (timer > -2)
        {
            textTimer.text = "00:00";
            timer -= Time.deltaTime;
        }
        else
        {
            textTimer.text = "--:--";
        }
    }

    IEnumerator LastSeconds()
    {
        anim.SetTrigger("Red");

        yield return new WaitForSeconds(1);

        if (timer > 0)
        {
            StartCoroutine("LastSeconds");
        }

    }

}

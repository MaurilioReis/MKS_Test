using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class FadeLight : MonoBehaviour
{
    [Header("Light to control")]
    public Light2D _light;

    [Space(10)]
    [Header("Max instensity")]
    public float maxIntensity = 1;

    [Space(10)]
    [Header("Timers to start function")]
    public float timeStart = 0;

    [Space(10)]
    [Header("Timers to control light fade")]
    public float timeFadeIn = 0.5f;
    public float timeWait = 0.5f;
    public float timeFadeOut = 0.5f;

    [Space(10)]
    [Header("End fade")]
    public bool DestroyGameObject = true;
    public bool DisabledScript = false;
    public bool DisabledLight = false;

    // Stage 0 (Stop) // Stage 1 (Start and Fade In) // Stage 2 (timer Wait) // Stage 3 (Time fade out to stop)
    private int stage = 0;


    public void OnEnable()
    {
        _light.enabled = true;
        _light.intensity = 0;
        StartCoroutine("StartControllerLight");
    }

    public IEnumerator StartControllerLight()
    {
        yield return new WaitForSeconds(timeStart);

        stage = 1;

        yield return new WaitForSeconds(timeFadeIn);

        stage = 2;

        yield return new WaitForSeconds(timeWait);

        stage = 3;

        yield return new WaitForSeconds(timeFadeOut); //End

        stage = 4;
    }

    private void FixedUpdate()
    {
        if(stage == 1) //Fade in
        {
            _light.intensity += Time.deltaTime * maxIntensity / timeFadeIn;
        }

        if (stage == 3 || stage == 4) //Fade out
        {
            _light.intensity -= Time.deltaTime * maxIntensity / timeFadeOut;
        }

        if (stage == 4 && _light.intensity <= 0)
        {
            if (DestroyGameObject)
            {
                Destroy(gameObject);
            }

            if (DisabledLight)
            {
                _light.enabled = false;
            }

            if (DisabledScript)
            {
                stage = 0;
                enabled = false;
            }
        }
    }
}

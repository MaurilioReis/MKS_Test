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

    public bool DestroyGameObject = true;
    public bool DisabledLight = false;

    // Stage 0 (Stop) // Stage 1 (Start and Fade In) // Stage 2 (timer Wait) // Stage 3 (Time fade out to stop) // Stage 4 (End)
    private int stage = 0;

    private void Start()
    {
        _light.intensity = 0;
        StartCoroutine("StartControllerLight");
        InvokeRepeating("AltereLight", 0, 0.1f);
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

        if (DestroyGameObject)
        {
            Destroy(gameObject);
        }
        else if (DisabledLight)
        {
            _light.enabled = false;
            enabled = false;
        }
    }

    private void AltereLight()
    {
        if(stage == 1) //Fade in
        {
            _light.intensity += maxIntensity / 10;
        }

        if (stage == 3) //Fade out
        {
            _light.intensity -= maxIntensity / 10;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class LightController : MonoBehaviour
{
    [Header("Light to control")]
    public Light2DBase _light;

    public float intensity;

    [Space(10)]
    [Header("Min and Max to instensity")]
    public float minIntensity = 0;
    public float maxIntensity = 1;

    [Space(10)]
    [Header("Timers to start function")]
    public float timeStart = 0;

    [Space(10)]
    [Header("Timers to control light fade")]
    public float timeFadeIn = 1;
    public float timeWait = 1;
    public float timeFadeOut = 1;

    // Stage 0 (Stop) // Stage 1 (Start and Fade In) // Stage 2 (timer Wait) // Stage 3 (Time fade out to stop) // Stage 4 (End)
    private int stage = 0;

    private void Start()
    {
        StartCoroutine("StartControllerLight");
    }

    public IEnumerator StartControllerLight()
    {
        yield return new WaitForSeconds(timeStart);
    }

    private void FixedUpdate()
    {
        if(stage == 1) { }
    }

}

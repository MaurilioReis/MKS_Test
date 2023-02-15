using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{

    [Header("Cams")]
    public CinemachineVirtualCamera cvCam;
    CinemachineBasicMultiChannelPerlin noise;

    [Header("Target")]
    public Transform[] targets;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CamController>();

        noise = cvCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetNewLookAt(int newTarget)
    {
        cvCam.Follow = targets[newTarget];
    }

    public void ShakeCam()
    {
        noise.m_FrequencyGain = 1;
    }

    private void FixedUpdate()
    {
        if (noise.m_FrequencyGain > 0)
        {
            noise.m_FrequencyGain -= Time.deltaTime;
        }
        else
        {
            noise.m_FrequencyGain = 0;
        }
    }
}

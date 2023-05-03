using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cbmcp;
    private float timer;

    void Start()
    {
        StopShake();
    }

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera(float shakeIntensity, float shakeTime)
    {
       
        CinemachineBasicMultiChannelPerlin cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = shakeIntensity;
        timer = shakeTime;

        // while(timer > 0)
        // {
        //     Debug.Log(timer);
        //     timer -= 0.1f;

        //     if(timer <= 0)
        //     {
        //         StopShake();
        //     }
        // }
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;
        timer = 0f;
    }
}

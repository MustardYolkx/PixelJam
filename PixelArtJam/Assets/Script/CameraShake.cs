using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance => instance;

    public CinemachineVirtualCamera virtualCamera;
    public float shakeIntensity = 1f;
    public float shakeTime = 0.2f;

    private float shakeTimer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private CinemachineImpulseSource _impulseSource;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
         _cbmcp = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopShake();
    }

    // Update is called once per frame
    void Update()
    {

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera(float shakeIntensity)
    {
        
        _cbmcp.m_AmplitudeGain = shakeIntensity;
        shakeTimer = shakeTime;
    }

    void StopShake()
    {
        
        _cbmcp.m_AmplitudeGain = 0f;
        shakeTimer = 0;
    }

    public void ShakeCameraCustom()
    {
        _impulseSource.GenerateImpulse();
    }
}

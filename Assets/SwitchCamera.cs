using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerVCam;
    [SerializeField] private CinemachineVirtualCamera whiteboardVCam;

    private bool usePlayerCam = true;

    public void SwitchPriority()
    {
        usePlayerCam = !usePlayerCam;
        if(usePlayerCam)
        {
            playerVCam.Priority = 1;
            whiteboardVCam.Priority = 0;
        }
        else
        {
            playerVCam.Priority = 0;
            whiteboardVCam.Priority = 1;
        }
    }
}

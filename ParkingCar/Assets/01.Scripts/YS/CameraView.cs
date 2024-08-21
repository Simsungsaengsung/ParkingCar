using Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _2dVcam;
    [SerializeField] private CinemachineVirtualCamera _3dVcam;

    private void Awake()
    {
        _2dVcam.Priority = 11;
        _3dVcam.Priority = 10;
        EventManager.AddListener<StartParkingEvent>(HandleStartParkingEvent);
    }

    private void HandleStartParkingEvent(StartParkingEvent evt)
    {
        _2dVcam.Priority = 10;
        _3dVcam.Priority = 11;
    }
}

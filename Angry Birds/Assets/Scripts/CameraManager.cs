using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _idleCamera;
    [SerializeField] private CinemachineCamera _followCamera;

    private void Awake()
    {
        SwitchToIdle();
    }

    public void SwitchToIdle()
    {
        _idleCamera.enabled = true;
        _followCamera.enabled = false;

    }

    public void SwitchToFollow(Transform followXform)
    {
        _followCamera.Follow = followXform;

        _idleCamera.enabled = false;
        _followCamera.enabled = true;

    }
}

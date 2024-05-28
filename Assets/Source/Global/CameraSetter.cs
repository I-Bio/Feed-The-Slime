using Cinemachine;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    private CinemachineTransposer _virtualCamera;
    private Vector3 _position;

    public void Initialize(CinemachineVirtualCamera virtualCamera, Vector3 position)
    {
        _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _position = position;
    }

    public void ChangeToGameView()
    {
        _virtualCamera.m_FollowOffset = _position;
    }
}
using Unity.Cinemachine;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public void ActivateCamera()
    {
        cinemachineCamera.Priority = 10;
    }

    public void DeactivateCamera()
    {
        cinemachineCamera.Priority = 0;
    }
}
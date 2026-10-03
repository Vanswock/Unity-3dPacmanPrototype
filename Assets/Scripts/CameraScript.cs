using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    private InputActionPlayer inputActions;
    private InputAction CameraChanger;
    public CinemachineVirtualCamera minimapCamera;
    public CinemachineVirtualCamera fpsCamera;
    private void Awake()
    {
        inputActions = new InputActionPlayer();
        CameraChanger = inputActions.Camera.ChangeCamera; ;
    }
    private void OnEnable()
    { 
        inputActions.Enable();
        CameraChanger.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
        CameraChanger.Disable();
    }
    void Update()
    {
        if (CameraChanger.triggered)
        {
            if (minimapCamera.Priority > fpsCamera.Priority)
            {
                minimapCamera.Priority = 5;
                fpsCamera.Priority = 10;
            }
            else
            {
                minimapCamera.Priority = 20;
                fpsCamera.Priority = 10;
            }
        }
    }
}

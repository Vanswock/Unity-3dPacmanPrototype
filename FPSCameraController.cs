using UnityEngine;
using UnityEngine.InputSystem;
public class FPSCameraController : MonoBehaviour
{
    public Transform cameraPivot;
    public float sensitivity = 0.1f;

    private InputActionPlayer inputActions;
    private InputAction look;

    float xRotation = 0f;

    void Awake()
    {
        inputActions = new InputActionPlayer();
        look = inputActions.FPV.Look;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        cameraPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseDelta = look.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);

        xRotation -= mouseDelta.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
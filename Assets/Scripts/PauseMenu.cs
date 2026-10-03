using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static InputActionPlayer;

public class PauseMenu : MonoBehaviour
{

    InputActionPlayer inputActions;
    InputAction pause;
    bool isPaused = false;
    public GameObject pauseImage;


    void Awake()
    {
        inputActions = new InputActionPlayer();
        pause = inputActions.Escape.Pausemenu;

    }
    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (pause.triggered)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0f;
                pauseImage.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                pauseImage.SetActive(false);
            }
        }
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        pauseImage.SetActive(false);
    }
}
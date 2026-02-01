using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuHolder;

    [SerializeField] GameObject pauseMenuMain;
    [SerializeField] GameObject optionsMenu;

    bool isActive = false;

    private int mainMenuSceneIndex = 0;

    private void Update()
    {
        //Replace with newer input system
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActive) ActivatePauseMenu();
            else TurnOffPauseMenu();
        }
    }

    private void ActivatePauseMenu()
    {
        if (Time.timeScale == 0) return;

        isActive = true;
        pauseMenuHolder.SetActive(true);
        Time.timeScale = 0f;
        ShowCursorCursor();

        if(pauseMenuMain != null && optionsMenu != null)
        {
            pauseMenuMain.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }

    public void TurnOffPauseMenu()
    {
        isActive = false;
        pauseMenuHolder.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1.0f;
        LockAndHideCursor();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
        Time.timeScale = 1.0f;
    }

    private void ShowCursorCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

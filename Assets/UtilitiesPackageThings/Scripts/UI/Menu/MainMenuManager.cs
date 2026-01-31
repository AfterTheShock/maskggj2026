using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup allMenusCanvasGroup;

    [SerializeField] CanvasGroup blackOutToNextSceneCanvasGroup;
    [SerializeField] float blackOutSpeed = 3.5f;

    [SerializeField] AudioSource[] allAudioSourcesToDampDownWithBackout;

    [SerializeField] GameObject firstSelectedButtonOnStart;

    private bool pressedPlayButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonOnStart);
    }

    private void Update()
    {
        if (pressedPlayButton) 
        {
            if (blackOutToNextSceneCanvasGroup != null) 
            {
                blackOutToNextSceneCanvasGroup.alpha += blackOutSpeed * Time.unscaledDeltaTime;

                foreach(AudioSource source in allAudioSourcesToDampDownWithBackout)
                {
                    source.volume -= blackOutSpeed * Time.unscaledDeltaTime;
                }
            }
            else GoToNextScene();


            if(blackOutToNextSceneCanvasGroup.alpha >= 1) GoToNextScene();
        }

        if(EventSystem.current.currentSelectedGameObject == null) EventSystem.current.SetSelectedGameObject(firstSelectedButtonOnStart);
    }

    public void PressPlayButton()
    {
        pressedPlayButton = true;
        if(allMenusCanvasGroup != null) allMenusCanvasGroup.interactable = false;
    }

    private void GoToNextScene()
    {
        int index = 1;

        if (SceneManager.GetActiveScene() != null) index = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(index);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}

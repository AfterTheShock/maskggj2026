using TMPro;
using UnityEngine;

public class FpsAndPerformanceUiManager : MonoBehaviour
{
    [Header("Debug Menu")]
    [SerializeField] GameObject debugMenu;

    [Header("FpsCounter")]
    public TextMeshProUGUI fpsText;
    [SerializeField] float pollingOrUpdateTime = 1;
    private float currentTime = 0;
    private int frameCount;
    private int resetAfterXIterations = 25; //To reset the current time to avoide dragged errors
    private int currentAmmountOfIterations = 0;

    private void Update()
    {
        if (debugMenu) DebugMenuOnOffManager();

        if (fpsText && debugMenu.activeSelf) CalculateFps();
    }

    private void DebugMenuOnOffManager()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            debugMenu.SetActive(!debugMenu.activeSelf);
        }
    }

    private void CalculateFps()
    {
        currentTime += Time.unscaledDeltaTime;

        frameCount++;

        if(currentTime >= pollingOrUpdateTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / currentTime);

            currentTime -= pollingOrUpdateTime;
            frameCount = 0;

            fpsText.text = "Fps: " + frameRate;

            resetAfterXIterations++;
            if(currentAmmountOfIterations >= resetAfterXIterations)
            {
                currentAmmountOfIterations = 0;
                currentTime = 0;
            }
        }
    }
}


using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider ambianceSlider;
    [SerializeField] Slider masterSlider;

    [SerializeField] GameObject firstSelectedButtonOnStart;

    [SerializeField] GameObject[] allOptionsBrackgrounds = new GameObject[0];

    [Header("GraphicsStuff")]
    private Resolution[] resolutions;
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TMP_Dropdown qualityDropdown;

    #region singletonPatern
    private static OptionsMenuManager _instance;
    public static OptionsMenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindFirstObjectByType<OptionsMenuManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void InicializeData()
    {
        InicializeResolutionParameters();

        SetSlidersValuesToSavedData();

        ToggleVsync(Options_DataToSave.Instance.vsync);

    }
    private void Start()
    {
        InicializeData();
    }

    private void InicializeResolutionParameters()
    {
        //Get resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        //Reversing resolutions so they are ordered properly
        Resolution[] tempArray = new Resolution[resolutions.Length];
        for (int i = 0; i < resolutions.Length; i++)
        {
            tempArray[resolutions.Length - 1 - i] = resolutions[i];
        }
        resolutions = tempArray;

        //Set the dropdown with the possible resolutions
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        //Add the resolutions to the dropdown
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);

        //Save data
        Options_DataToSave.Instance.resolutionIndex = resolutionIndex;
        Options_DataToSave.Instance.SaveData();

        //Update drowdown
        resolutionDropdown.value = resolutionIndex;
    }

    public void ToggleVsync(bool vsyncEnabled)
    {   
        QualitySettings.vSyncCount = (vsyncEnabled ? 1 : 0);

        //Save data
        Options_DataToSave.Instance.vsync = vsyncEnabled;
        Options_DataToSave.Instance.SaveData();

        //Update toggle
        vsyncToggle.isOn = vsyncEnabled;
    }

    public void SetQualityLevel(int qualityIndex)
    {
        if (QualitySettings.count <= qualityIndex) return;

        QualitySettings.SetQualityLevel(qualityIndex);

        //Save data
        Options_DataToSave.Instance.qualityIndex = qualityIndex;
        Options_DataToSave.Instance.SaveData();

        //Update drowdown
        qualityDropdown.value = qualityIndex;
    }

    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        //Save data
        Options_DataToSave.Instance.fullscreen = isFullScreen;
        Options_DataToSave.Instance.SaveData();

        //Update toggle
        fullscreenToggle.isOn = isFullScreen;
    }

    private void OnEnable()
    {
        //Initialize a selected button to navegate with arrows
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonOnStart);
    }

    private void Update()
    {
        //Always have a selected ui element so you can navegate with the arrows or joistick
        if (EventSystem.current.currentSelectedGameObject == null) EventSystem.current.SetSelectedGameObject(firstSelectedButtonOnStart);
    }

    public void OnChangeMusicSlider()
    {

        float volume = LinearToDecibel(musicSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("MusicVolume", volume);

        Options_DataToSave.Instance.musicVolume = musicSlider.value;
        ChangedVolume();
    }

    public void OnChangeSFXSlider()
    {

        float volume = LinearToDecibel(SFXSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("SFXVolume", volume);

        Options_DataToSave.Instance.sfxVolume = SFXSlider.value;
        ChangedVolume();
    }

    public void OnChangeAmbianceSlider()
    {

        float volume = LinearToDecibel(ambianceSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("AmbianceVolume", volume);

        Options_DataToSave.Instance.ambianceVolume = ambianceSlider.value;
        ChangedVolume();
    }

    public void OnChangeMasterSlider()
    {

        float volume = LinearToDecibel(masterSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("MasterVolume", volume);

        Options_DataToSave.Instance.masterVolume = masterSlider.value;
        ChangedVolume();
    }

    private void ChangedVolume()
    {
        Options_DataToSave.Instance.SaveData();
    }

    public float DecibelToLinear(float db)
    {
        // If the value is -80dB (Unity's silence), just return 0 to avoid tiny floating point numbers.
        if (db <= -80f) return 0f;

        return Mathf.Pow(10.0f, db / 20.0f);
    }

    public float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f) return -80f; // Clamp silence

        return Mathf.Log10(linear) * 20.0f;
    }

    private void SetSlidersValuesToSavedData()
    {
        masterSlider.value = Options_DataToSave.Instance.masterVolume;
        musicSlider.value = Options_DataToSave.Instance.musicVolume;
        SFXSlider.value = Options_DataToSave.Instance.sfxVolume;
        ambianceSlider.value =Options_DataToSave.Instance.ambianceVolume;
    }

    public void TurnOffAllOptionsBackgrounds()
    {
        foreach(GameObject o in allOptionsBrackgrounds)
        {
            o.SetActive(false);
        }
    }
}

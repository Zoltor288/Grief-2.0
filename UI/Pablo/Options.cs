using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public UnityEngine.UI.Toggle toggleFS;
    public UnityEngine.UI.Toggle toggleSound;
    public GameObject IntroCanvas;
    public GameObject OptionsCanvas;
    public UnityEngine.UI.Slider sliderBrightness;
    public UnityEngine.UI.Slider sliderMaster;
    public UnityEngine.UI.Slider sliderSoundFX;
    public UnityEngine.UI.Slider sliderMusic;
    public float sliderValue;
    public UnityEngine.UI.Image panelBrightness;
    public TMP_Dropdown resolutionDropDown;
    Resolution[] resolutions;

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(panelBrightness.gameObject);
    }

    private void Start()
    {
        IntroCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
        sliderBrightness.value = PlayerPrefs.GetFloat("brightness", 0.5f);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.b, panelBrightness.color.b, sliderValue);
        if (Screen.fullScreen)
        {
            toggleFS.isOn = true;
        }
        else
        {
            toggleFS.isOn = false;
        }

        CheckResolution();

        if (PlayerPrefs.HasKey("qualityIndex"))
        {
            int index = PlayerPrefs.GetInt("qualityIndex");
            ChangeQuality(index);
        }
        if (PlayerPrefs.HasKey("numberResolution"))
        {
            int index = PlayerPrefs.GetInt("numberResolution");
            ChangeResolution(index);
            resolutionDropDown.value = index;
        }
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("fullscreen") == 1;
            Screen.fullScreen = isFullScreen;
            toggleFS.isOn = isFullScreen;
        }

        if (PlayerPrefs.HasKey("MusicOn"))
        {
            bool isMusicOn = PlayerPrefs.GetInt("MusicOn") == 1;
            toggleSound.isOn = !isMusicOn;
            SetMute(toggleSound.isOn);
        }
        if (PlayerPrefs.HasKey("volumemaster"))
        {
            float masterVol = PlayerPrefs.GetFloat("volumemaster");
            ChangeVolumeMaster(masterVol);
            sliderMaster.value = masterVol;
        }
        if (PlayerPrefs.HasKey("volumesfx"))
        {
            float sfxVol = PlayerPrefs.GetFloat("volumesfx");
            ChangeVolumeSFX(sfxVol);
            sliderSoundFX.value = sfxVol;
        }
        if (PlayerPrefs.HasKey("volumemusic"))
        {
            float musicVol = PlayerPrefs.GetFloat("volumemusic");
            ChangeVolumeMusic(musicVol);
            sliderMusic.value = musicVol;
        }
    }

    #region Volume
    public void SetVolume(string parameterName, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameterName, dB);
    }

    public void MuteGroup()
    {
        audioMixer.SetFloat("VolumeMaster", -80f);
    }
    public void UnmuteGroup()
    {
        audioMixer.SetFloat("VolumeMaster", 0f);
    }
    public void SetMute(bool isOn)
    {
        if (isOn)
        {
            MuteGroup(); // Silenciar música
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            UnmuteGroup(); // Restaurar volumen
            PlayerPrefs.SetInt("MusicOn", 1);
        }
    }
    public void ChangeVolumeMaster(float volumemaster)
    {
        SetVolume("VolumeMaster", volumemaster);
        PlayerPrefs.SetFloat("volumemaster", volumemaster);
    }

    public void ChangeVolumeSFX(float volumesfx)
    {
        SetVolume("VolumeSFX", volumesfx);
        PlayerPrefs.SetFloat("volumesfx", volumesfx);
    }
    public void ChangeVolumeMusic(float volumemusic)
    {
        SetVolume("VolumeMusic", volumemusic);
        PlayerPrefs.SetFloat("volumemusic", volumemusic);
    }

    #endregion
    #region Screen
    public void SetFullScreen(bool setfullScreen)
    {
        Screen.fullScreen = setfullScreen;
        PlayerPrefs.SetInt("fullscreen", setfullScreen ? 1 : 0);
    }

    public void ChangeBrightness(float brightness)
    {
        sliderValue = brightness;
        PlayerPrefs.SetFloat("brightness", sliderValue);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.b, panelBrightness.color.b, sliderValue);
    }
    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("qualityIndex", index);
    }

    public void CheckResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string opcion = resolutions[i].width + " x " + resolutions[i].height;
            opciones.Add(opcion);


            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }

        }

        resolutionDropDown.AddOptions(opciones);
        resolutionDropDown.value = resolucionActual;
        resolutionDropDown.RefreshShownValue();

        resolutionDropDown.value = PlayerPrefs.GetInt("numberResolution", 0);
    }

    public void ChangeResolution(int indexResolution)
    {
        PlayerPrefs.SetInt("numberResolution", resolutionDropDown.value);

        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    #endregion
    #region Buttons
    public void Back()
    {
        IntroCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
    }
    public void OptionsSelection()
    {
        IntroCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
    }
    #endregion
}

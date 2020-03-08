﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;

    public ResolutionItem[] resolutions;

    public int selectedResolution = 0;

    public Text resolutionLabel;

    public AudioMixer mixer;

    public Slider masterSlider, musicSlider, sfxSlider;

    public Text masterLabel, musicLabel, sfxLabel;

    // Start is called before the first frame update
    void Start()
    {
        setupGraphics();
        setupVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void decreaseResolution()
    {
        selectedResolution = selectedResolution > 0 ? --selectedResolution : 0;

        updateResolutionText();
    }

    public void increaseResolution()
    {
        var maxLength = resolutions.Length - 1;
        selectedResolution = selectedResolution < maxLength ? ++selectedResolution : maxLength;

        updateResolutionText();
    }

    public void updateResolutionText()
    {
        resolutionLabel.text = $"{resolutions[selectedResolution].width.ToString()} x {resolutions[selectedResolution].height.ToString()}";
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenToggle.isOn;

        if (vsyncToggle.isOn) {
            QualitySettings.vSyncCount = 1;
        } else {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].width, resolutions[selectedResolution].height, fullscreenToggle.isOn);
    }

    public void setMasterVolume()
    {
        setLocalMasterVolume(masterSlider.value);
    }

    public void setMusicVolume()
    {
        setLocalMusicVolume(musicSlider.value);
    }

    public void setSFXVolume()
    {
        setLocalSFXVolume(sfxSlider.value);
    }

    private void setupGraphics()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0) {
            vsyncToggle.isOn = false;
        } else {
            vsyncToggle.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height) {
                foundRes = true;
                selectedResolution = i;
                updateResolutionText();
                break;
            }
        }

        if (!foundRes) {
            resolutionLabel.text = $"{Screen.width.ToString()} x {Screen.height.ToString()}";
        }
    }

    private void setupVolume()
    {
        if (PlayerPrefs.HasKey("MasterVol")) {
            setLocalMasterVolume(PlayerPrefs.GetFloat("MasterVol"));
        } else {
            setMasterVolume();
        }

        if (PlayerPrefs.HasKey("MusicVol")) {
            setLocalMusicVolume(PlayerPrefs.GetFloat("MusicVol"));
        } else {
            setMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVol")) {
            setLocalSFXVolume(PlayerPrefs.GetFloat("SFXVol"));
        } else {
            setSFXVolume();
        }
    }

    private void setLocalMasterVolume(float value)
    {
        masterLabel.text = (value + 80).ToString();
        masterSlider.value = value;
        mixer.SetFloat("MasterVol", value);
        PlayerPrefs.SetFloat("MasterVol", value);
    }

    private void setLocalMusicVolume(float value)
    {
        musicLabel.text = (value + 80).ToString();
        musicSlider.value = value;
        mixer.SetFloat("MusicVol", value);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    private void setLocalSFXVolume(float value)
    {
        sfxLabel.text = (value + 80).ToString();
        sfxSlider.value = value;
        mixer.SetFloat("SFXVol", value);
        PlayerPrefs.SetFloat("SFXVol", value);
    }
}

[System.Serializable]
public class ResolutionItem
{
    public int width, height;
}

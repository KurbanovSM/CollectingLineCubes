using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeSoundVolume : MonoBehaviour
{
    private float SoundVolume 
    {
        get => PlayerPrefs.GetFloat("SoundVolume", .25f); 
        set => PlayerPrefs.SetFloat("SoundVolume", value); 
    }

    [SerializeField] private TMP_Text soundVolumeText;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private List<AudioSource> audioSources;

    private void Awake()
    {
        soundSlider.value = SoundVolume;
        audioSources[0].volume = SoundVolume;
        audioSources[1].volume = SoundVolume;

        UpdateSoundVolumeText();
    }

    public void Change()
    {
        SoundVolume = soundSlider.value;
        audioSources[0].volume = SoundVolume;
        audioSources[1].volume = SoundVolume;
        UpdateSoundVolumeText();
    }

    private void UpdateSoundVolumeText()
    {
        soundVolumeText.text = $"Sound volume\n%{(SoundVolume*100).ToString("F0")}";
    }

}

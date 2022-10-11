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
    [SerializeField] private Slider soundlider;

    [SerializeField] private List<AudioSource> audioSources;

    private void Awake()
    {
        soundlider.value = SoundVolume;
        audioSources[0].volume = SoundVolume;
        audioSources[1].volume = SoundVolume;

        UpdateSoundVolumeText();
    }

    public void Change()
    {
        SoundVolume = soundlider.value;
        audioSources[0].volume = SoundVolume;
        audioSources[1].volume = SoundVolume;
        UpdateSoundVolumeText();
    }

    private void UpdateSoundVolumeText()
    {
        soundVolumeText.text = $"Sound volume\n%{(SoundVolume*100).ToString("F0")}";
    }

}

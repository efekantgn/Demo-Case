using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;

    const string MIXER_MASTER = "Master";
    const string MIXER_MUSIC = "Music";
    const string MIXER_SOUND = "Sound";

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(OnMasterSliderValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
    }

    private void OnSoundSliderValueChanged(float arg0)
    {
        gameMixer.SetFloat(MIXER_SOUND, Mathf.Log10(arg0) * 20);

    }

    private void OnMasterSliderValueChanged(float arg0)
    {
        gameMixer.SetFloat(MIXER_MASTER, Mathf.Log10(arg0) * 20);

    }

    private void OnMusicSliderValueChanged(float arg0)
    {
        gameMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(arg0) * 20);
    }
}

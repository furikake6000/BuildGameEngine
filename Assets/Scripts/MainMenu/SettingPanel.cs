using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingPanel : MonoBehaviour {

    [SerializeField]
    Slider bgmVolumeSlider, seVolumeSlider;
    [SerializeField]
    AudioMixer audioMixer;

	// Use this for initialization
	void Start () {
        //現在の音量をスライドバーに反映
        float getParam;
        audioMixer.GetFloat("BgmVolume", out getParam);
        bgmVolumeSlider.value = getParam;
        audioMixer.GetFloat("SeVolume", out getParam);
        seVolumeSlider.value = getParam;
    }
	
    // Eventから呼ばれる用メソッド

	public void SetBgmVolume()
    {
        audioMixer.SetFloat("BgmVolume", bgmVolumeSlider.value);
    }

    public void SetSeVolume()
    {
        audioMixer.SetFloat("SeVolume", seVolumeSlider.value);
    }
}

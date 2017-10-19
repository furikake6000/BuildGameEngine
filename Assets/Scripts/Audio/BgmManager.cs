using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BgmManager : MonoBehaviour {

    static AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// BGMを変更する
    /// （Audioシーンを作成した直後に呼ばない！1フレーム待つこと！）
    /// </summary>
    /// <param name="source">変更先BGM</param>
    public static void ChangeBgm(AudioClip source)
    {
        //audioSourceが未だロードされてなければ早すぎる呼び出し、不適切
        Assert.IsNotNull(audioSource, "Don't call ChangeBgm() before audioscene loaded!");
        audioSource.clip = source;
        audioSource.Play();
    }
    /// <summary>
    /// BGMを停止する
    /// （Audioシーンを作成した直後に呼ばない！1フレーム待つこと！）
    /// </summary>
    public static void StopBgm()
    {
        //audioSourceが未だロードされてなければ早すぎる呼び出し、不適切
        Assert.IsNotNull(audioSource, "Don't call StopBgm() before audioscene loaded!");
        audioSource.Stop();
    }
}

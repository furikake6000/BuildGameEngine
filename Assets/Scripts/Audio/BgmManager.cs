using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour {

    static AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void ChangeBgm(AudioClip source)
    {
        audioSource.clip = source;
        audioSource.Play();
    }
}

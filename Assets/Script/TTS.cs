﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTS : Singleton<TTS>
{
    [SerializeField] public AudioSource audioSource;
    [HideInInspector] public string url;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //StartCoroutine(DownloadTheAudio());
    }

    public void SetAudio(string vocab)
    {
        url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=" 
              + vocab.Length +
              "&client=tw-ob&q=" 
              + vocab + "&tl=En-gb";
    }

    public void PlayAudio()
    {
        StartCoroutine(DownloadTheAudio());
    }
    IEnumerator DownloadTheAudio()
    {
        //string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=SampleText&tl=En-gb";
        WWW www = new WWW(url);
        yield return www;

        audioSource.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        audioSource.Play();
    }
}
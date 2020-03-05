﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] soundArray;

    // Start is called before the first frame update
    void Awake()
    {
        //sets all audio clips when game starts 
        foreach (Sound s in soundArray)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "MainGame")
        {
            //background music is played instead of playOneShot 
            Sound s = Array.Find(soundArray, sound => sound.name == "Theme");
            s.source.Play();
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(soundArray, sound => sound.name == name);
        if (s != null)
        {
            s.source.PlayOneShot(s.clip);
        }
        else
        {
            Debug.Log("Missing audio file" + name);
        }
    }
}
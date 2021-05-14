using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the base structure for an audio clip
/// </summary>
[Serializable]
public class Audioclip {
    public AudioType _AudioType;
    public AudioClip _Audio;
}

/// <summary>
/// This enum holds all the types of sounds used in the project
/// </summary>
public enum AudioType {
    TAP,
    WIN,
    BACK,
    INSTRUCTION_CHANGE,
    DRILL,
    METALATTACH

}

/// <summary>
/// This class contains all the methods used for sound management 
/// </summary>
public class SoundManager : Singleton<SoundManager> {
    [Header("Audio Source")] public AudioSource _AudioSource;

    [Header("Sounds Collection")] public List<Audioclip> Audioclips;
    Dictionary<AudioType, AudioClip> AudioDictionary;

    public override void OnAwake() {
        base.OnAwake();
        PrepareAudioDictionary();
    }


    /// <summary>
    /// This methods prepares an audio dictonary at the begining
    /// </summary>
    public void PrepareAudioDictionary() {
        AudioDictionary = new Dictionary<AudioType, AudioClip>();

        foreach (var audioclip in Audioclips) {
            AudioDictionary.Add(audioclip._AudioType, audioclip._Audio);
        }
    }

    /// <summary>
    /// This methods plays an audio
    /// </summary>
    public void PlayAudio(AudioType audioType) {
        _AudioSource.PlayOneShot(AudioDictionary[audioType]);
    }

    /// <summary>
    /// This methods retuns an audio clip
    /// </summary>
    public AudioClip GetAudioClip(AudioType audioType)
    {
        return AudioDictionary[audioType];
    }

    /// <summary>
    /// This methods plays TAP audio for UI
    /// </summary>
    public void PlayTap() {
        PlayAudio(AudioType.TAP);
    }

    /// <summary>
    /// This methods plays Back audio for UI
    /// </summary>
    public void PlayBack() {
        PlayAudio(AudioType.BACK);
    }
}
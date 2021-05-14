using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Audioclip {
    public AudioType _AudioType;
    public AudioClip _Audio;
}

public enum AudioType {
    TAP,
    WIN,
    BACK,
    INSTRUCTION_CHANGE,
    DRILL,
    METALATTACH

}

public class SoundManager : Singleton<SoundManager> {
    [Header("Audio Source")] public AudioSource _AudioSource;

    [Header("Sounds Collection")] public List<Audioclip> Audioclips;
    Dictionary<AudioType, AudioClip> AudioDictionary;

    public override void OnAwake() {
        base.OnAwake();
        PrepareAudioDictionary();
    }

    public void PrepareAudioDictionary() {
        AudioDictionary = new Dictionary<AudioType, AudioClip>();

        foreach (var audioclip in Audioclips) {
            AudioDictionary.Add(audioclip._AudioType, audioclip._Audio);
        }
    }

    public void PlayAudio(AudioType audioType) {
        _AudioSource.PlayOneShot(AudioDictionary[audioType]);
    }
    public AudioClip GetAudioClip(AudioType audioType)
    {
        return AudioDictionary[audioType];
    }

    public void PlayTap() {
        PlayAudio(AudioType.TAP);
    }

    public void PlayBack() {
        PlayAudio(AudioType.BACK);
    }
}
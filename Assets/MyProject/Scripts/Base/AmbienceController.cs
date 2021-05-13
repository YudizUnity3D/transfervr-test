using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrasnferVR.Demo {
    public class AmbienceController : Singleton<AmbienceController> {
        [Header("Audio Clips")]
        public AudioClip UIMusic;
        public AudioClip SimulationAudio;

        [Header("Audio Source")]
        public AudioSource _AudioSource;

        public Ambience currentAmbience;

        public enum Ambience {
            None,
            UI,
            Simulation
        }

        private void OnEnable() {

            Events.OnSimulationStateChanged += OnSimulationStateChanged;
        }

        private void OnDisable() {
            Events.OnSimulationStateChanged -= OnSimulationStateChanged;

        }

        private void OnSimulationStateChanged(Data.SimulationState obj) {
            throw new NotImplementedException();
        }

        public void SwitchAmbience(Ambience _ambience) {
            StartCoroutine(SetAmbienceRoutine(_ambience));
        }

        IEnumerator SetAmbienceRoutine(Ambience ambience) {

            float timeElapsed = 0;
            float lerpDuration = 0.5f;

            float startVolume = _AudioSource.volume;
            float endVolume = 0;

            if (currentAmbience != ambience) {


                while (timeElapsed < lerpDuration) {
                    _AudioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
            }

            yield return null;


            if (ambience == Ambience.UI) {
                _AudioSource.clip = UIMusic;
            } else {
                _AudioSource.clip = SimulationAudio;
            }


            timeElapsed = 0;
            lerpDuration = 0.5f;

            startVolume = _AudioSource.volume;
            endVolume = 1;

            if (currentAmbience != ambience) {


                while (timeElapsed < lerpDuration) {
                    _AudioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
            }

        }

    }

}
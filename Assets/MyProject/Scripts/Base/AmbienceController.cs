using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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


        public void SwitchAmbience(Ambience _ambience) {
            if (currentAmbience != _ambience) {
                currentAmbience = _ambience;
                StartCoroutine(SetAmbienceRoutine());
            }
        }

        IEnumerator SetAmbienceRoutine() {


            if (_AudioSource.volume > 0f) {

                yield return _AudioSource.DOFade(0, 1f);

            }

            if (currentAmbience == Ambience.UI) {
                _AudioSource.clip = UIMusic;
            } else {
                _AudioSource.clip = SimulationAudio;
            }
            _AudioSource.Play();
            yield return _AudioSource.DOFade(1, 1f);


        }

        public void PauseAmbience() {
            _AudioSource.DOFade(0, 1f).onComplete = () => {

            };
        }

        public void ResumeAmbience() {
            _AudioSource.DOFade(1, 1f).onComplete = () => {

            };
        }

    }

}
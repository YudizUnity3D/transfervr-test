using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace TrasnferVR.Demo {
    /// <summary>
    /// This class contains the contol methods for managing the Ambience audio
    /// </summary>
    public class AmbienceController : Singleton<AmbienceController> {
        [Header("Audio Clips")]
        public AudioClip UIMusic;
        public AudioClip SimulationAudio;

        [Header("Audio Source")]
        public AudioSource _AudioSource;

        public Ambience currentAmbience;

        /// <summary>
        /// This enum holds the types of enums
        /// </summary>
        public enum Ambience {
            None,
            UI,
            Simulation
        }

        /// <summary>
        /// This methods switches the ambience
        /// </summary>
        public void SwitchAmbience(Ambience _ambience) {
            if (currentAmbience != _ambience) {
                currentAmbience = _ambience;
                StartCoroutine(SetAmbienceRoutine());
            }
        }

        /// <summary>
        /// Routine for switching the ambience
        /// </summary>
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

        /// <summary>
        /// Method for pausing the ambiance sound/music
        /// </summary>
        public void PauseAmbience() {
            _AudioSource.DOFade(0, 1f).onComplete = () => {

            };
        }


        /// <summary>
        /// Method for resuming the ambiance sound/music
        /// </summary>
        public void ResumeAmbience() {
            _AudioSource.DOFade(1, 1f).onComplete = () => {

            };
        }

    }

}
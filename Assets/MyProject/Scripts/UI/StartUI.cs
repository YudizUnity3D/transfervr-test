using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {

    /// <summary>
    /// This class contains all the behaviors for the Start Screen 
    /// </summary>
    public class StartUI : ScreenView {

        #region UI_BASE_OVERRIDES

        public override void OnScreenShowCalled() {
            base.OnScreenShowCalled();

            AmbienceController.instance.SwitchAmbience(AmbienceController.Ambience.UI);
        }

        public override void OnScreenHideCalled() {
            base.OnScreenHideCalled();
        }

        #endregion


        #region BUTTON_METHODS

        public void OnStartPressed() {
            UIController.instance.HideThisScreen(ScreenType.START, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.INSTRUCTIONS, EnableDirection.Forward);
            SoundManager.instance.PlayTap();
        }


        public void OnQuitPressed() {
            Application.Quit();
        }
        #endregion
    }
}
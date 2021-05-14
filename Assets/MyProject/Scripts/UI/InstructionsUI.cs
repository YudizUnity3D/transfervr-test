using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {

    /// <summary>
    /// This class contains all the behaviors for the Instructions Screen 
    /// </summary>
    public class InstructionsUI : ScreenView {



        #region UI_BASE_OVERRIDES
        public override void OnScreenShowCalled() {
            base.OnScreenShowCalled();
          
        }

        public override void OnScreenHideCalled() {

            base.OnScreenHideCalled();
       
        }
        #endregion


        #region UI_METHODS
        public void OnStartPressed() {
            UIController.instance.HideThisScreen(ScreenType.INSTRUCTIONS, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            SoundManager.instance.PlayTap();
            Events.ChangeSimulationState(SimulationState.SIMULATION);

        }

        public void onBackPressed() {
            UIController.instance.HideThisScreen(ScreenType.INSTRUCTIONS, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.START, EnableDirection.Forward);
            SoundManager.instance.PlayTap();
        }
        #endregion

      


    }
}
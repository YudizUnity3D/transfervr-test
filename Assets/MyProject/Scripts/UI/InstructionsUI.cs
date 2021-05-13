using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {
    public class InstructionsUI : ScreenView {


        public override void OnScreenShowCalled() {
            base.OnScreenShowCalled();

        }

        public override void OnScreenHideCalled() {
            base.OnScreenHideCalled();

        }

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


    }
}
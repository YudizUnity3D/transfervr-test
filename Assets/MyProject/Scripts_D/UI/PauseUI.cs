using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {
    public class PauseUI : ScreenView {

        public override void OnScreenShowCalled() {
            base.OnScreenShowCalled();
        }

        public override void OnScreenHideCalled() {
            base.OnScreenHideCalled();
        }

        public void onRestartPressed() {

            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.START_SIMULATION);
            SoundManager.instance.PlayTap();

        }

        public void onResumePressed() {

            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.START_SIMULATION);
            SoundManager.instance.PlayTap();

        }

        public void onHomePressed() {

            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.START, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.UI);
            SoundManager.instance.PlayTap();

        }

        public override void OnBackKeyPressed() {
            base.OnBackKeyPressed();
            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.START_SIMULATION);
            SoundManager.instance.PlayBack();

        }

    }
}
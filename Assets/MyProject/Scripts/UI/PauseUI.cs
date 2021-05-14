using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI
{

    /// <summary>
    /// This class contains all the behaviors for the Pause Screen 
    /// </summary>
    public class PauseUI : ScreenView
    {
        #region UI_BASE_OVERRIDES
        public override void OnScreenShowCalled()
        {
            AmbienceController.instance.PauseAmbience();
            base.OnScreenShowCalled();
        }

        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            AmbienceController.instance.ResumeAmbience();

        }

        public override void OnBackKeyPressed() {
            base.OnBackKeyPressed();
            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.SIMULATION);
            SoundManager.instance.PlayBack();

        }
        #endregion


        #region UI_METHODS

        public void onRestartPressed()
        {

            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            CameraFadeEffect.instance.FadeInOut(() =>
            {
                Events.ResetEnvironment();
                Events.ChangeSimulationState(SimulationState.SIMULATION);
            });
            SoundManager.instance.PlayTap();

        }

        public void onResumePressed()
        {

            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.SIMULATION);
            SoundManager.instance.PlayTap();

        }

        public void onHomePressed()
        {

            UIController.instance.HideThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.START, EnableDirection.Forward);
            CameraFadeEffect.instance.FadeInOut(() =>
            {
                Events.ResetEnvironment();
                Events.ChangeSimulationState(SimulationState.UI);
            });
            SoundManager.instance.PlayTap();

        }
        #endregion


    }
}
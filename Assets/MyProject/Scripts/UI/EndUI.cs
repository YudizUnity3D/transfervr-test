using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI
{
    /// <summary>
    /// This class contains all the behaviors for the End Screen 
    /// </summary>
    public class EndUI : ScreenView
    {
        #region UI_BASE_OVERRIDES
        public override void OnScreenShowCalled()
        {
            SoundManager.instance.PlayAudio(AudioType.WIN);
            base.OnScreenShowCalled();
            AmbienceController.instance.SwitchAmbience(AmbienceController.Ambience.UI);
        }

        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
        }
        #endregion


        #region UI_METHODS

        public void onRestartPressed()
        {
            UIController.instance.HideThisScreen(ScreenType.END, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            CameraFadeEffect.instance.FadeInOut(() =>
            {
                Events.ResetEnvironment();
                Events.ChangeSimulationState(SimulationState.SIMULATION);
            });
            SoundManager.instance.PlayTap();

        }

        public void onHomePressed()
        {
            UIController.instance.HideThisScreen(ScreenType.END, EnableDirection.Forward);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI
{
    public class EndUI : ScreenView
    {

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
    }
}
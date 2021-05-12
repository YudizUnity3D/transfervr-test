using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;
using UnityEngine.XR.Interaction.Toolkit;

namespace TrasnferVR.Demo.UI
{
    public class ActiveUI : ScreenView
    {

        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
        }

        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
        }


        public override void OnBackKeyPressed()
        {
            base.OnBackKeyPressed();
            UIController.instance.HideThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            SoundManager.instance.PlayBack();
            Events.ChangeSimulationState(SimulationState.UI);

        }

//         public void Tick()
//         {
// #if !UNITY_EDITOR
//             XRController leftController = ControllerManager.instance.GetLeftController();
//             if (leftController != null && leftController.inputDevice.isValid)
//             {
//                 leftController.inputDevice.IsPressed(InputHelpers.Button.MenuButton, out bool value);
//                 if (value)
//                 {
//                     OnBackKeyPressed();
//                 }
//             }
// #endif
//         }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {
    public class StartUI : ScreenView {

        public override void OnScreenShowCalled() {
            base.OnScreenShowCalled();
        }

        public override void OnScreenHideCalled() {
            base.OnScreenHideCalled();
        }

        public void OnStartPressed() {
            UIController.instance.HideThisScreen(ScreenType.START, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            SoundManager.instance.PlayTap();
        }

        public void OnQuitPressed() {
            Application.Quit();
        }
    }
}
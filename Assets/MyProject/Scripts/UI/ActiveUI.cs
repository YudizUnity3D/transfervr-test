using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {
    public class ActiveUI : ScreenView {

        public CanvasGroup Instruction_CG;

        public Text Instruction_Text;

        [Header("Instruction Texts")]
        public string Instruction_DrillPick = "Pick the drill with one controller and pneumatic hose with another and attach the hose to the drill.";
        public string Instruction_Drilling = "Screw the board placed on the table using the drill.";


        [Header("Flags")]
        public InstuctionType currentInstuction = InstuctionType.NONE;
        public bool isResetClicked;
        public bool isHoseAttached;


        public override void OnScreenShowCalled() {
            base.OnScreenShowCalled();

            ResetInstructions();

            Events.OnHoseAttached += OnHoseAttached;
            Events.OnResetEnvironment += OnResetEnvironment;
            Events.OnTaskCompleted += OnTaskCompleted;

            AmbienceController.instance.SwitchAmbience(AmbienceController.Ambience.Simulation);


        }

        public override void OnScreenHideCalled() {
            base.OnScreenHideCalled();
            Events.OnHoseAttached -= OnHoseAttached;
            Events.OnResetEnvironment -= OnResetEnvironment;
            Events.OnTaskCompleted -= OnTaskCompleted;

        }

        public void ResetInstructions(bool hasToHide = false) {

            Instruction_CG.alpha = 0;
            Instruction_CG.blocksRaycasts = false;
            Instruction_CG.interactable = false;
            Instruction_Text.text = "";
            currentInstuction = InstuctionType.NONE;


            if (!hasToHide) {
                StartCoroutine(ShowInstructionRoutine());
            } else {
                HideInstruction();
            }

        }

        IEnumerator ShowInstructionRoutine() {

            yield return new WaitForSeconds(3f);


            if (isHoseAttached) {
                SetInstruction(InstuctionType.DRILLING);

            } else {
                SetInstruction(InstuctionType.SETUP_EQUIPMENTS);

            }
        }



        private void OnTaskCompleted() {
            HideInstruction();
        }

        public void ShowInstruction(Action Callback = null) {

            SoundManager.instance.PlayAudio(AudioType.INSTRUCTION_CHANGE);

            Instruction_CG.DOFade(1, 0.5f).onComplete = () => {
                Callback();
            };

        }

        public void HideInstruction(Action Callback = null) {
            Instruction_CG.DOFade(0, 0.5f).onComplete = () => {
                Callback();
            };
        }

        public void SetInstruction(InstuctionType type) {

            currentInstuction = type;

            if (Instruction_CG.alpha > 0f) {
                HideInstruction(() => {

                    switch (currentInstuction) {
                        case InstuctionType.SETUP_EQUIPMENTS:
                            Instruction_Text.text = Instruction_DrillPick;
                            break;

                        case InstuctionType.DRILLING:
                            Instruction_Text.text = Instruction_Drilling;
                            break;
                    }

                    ShowInstruction();
                });
            } else {

                switch (currentInstuction) {
                    case InstuctionType.SETUP_EQUIPMENTS:
                        Instruction_Text.text = Instruction_DrillPick;
                        break;

                    case InstuctionType.DRILLING:
                        Instruction_Text.text = Instruction_Drilling;
                        break;

                }

                ShowInstruction();

            }

        }

        private void OnResetEnvironment() {
            isResetClicked = true;
            isHoseAttached = false;
            ResetInstructions(true);
        }

        private void OnHoseAttached() {
            isHoseAttached = true;
            SetInstruction(InstuctionType.DRILLING);

        }


        public override void OnBackKeyPressed() {
            base.OnBackKeyPressed();
            UIController.instance.HideThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.PAUSE, EnableDirection.Forward);
            SoundManager.instance.PlayBack();
            Events.ChangeSimulationState(SimulationState.UI);
        }


        public enum InstuctionType {

            NONE,
            SETUP_EQUIPMENTS,
            DRILLING

        }
    }
}

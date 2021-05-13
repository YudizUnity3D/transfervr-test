using System;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {


    public class UIController : MonoBehaviour {
        public static UIController instance;
        [Header("Screens")]
        [SerializeField] List<ScreenCollection> _allScreens = null;

        [Header("Current")]
        public ScreenType _currentScreen;
        public ScreenType _previousScreen;


        private void Awake() {
            instance = this;
        }
        private void OnEnable() {
            Events.OnTaskCompleted += OnTaskCompleted;
        }
        private void OnDisable() {
            Events.OnTaskCompleted -= OnTaskCompleted;
        }

        private void Start() {
            ShowThisScreen(ScreenType.START, EnableDirection.Forward);
        }

        public void ShowThisScreen(ScreenType _screenToShow, EnableDirection _direction, Action _tempAction = null) {
            _previousScreen = _currentScreen;
            ScreenView m_screen = FindScreen(_screenToShow);
            _currentScreen = _screenToShow;
            m_screen.Show(_direction);
        }

        public void HideThisScreen(ScreenType _screenToHide, EnableDirection _direction, Action _tempAction = null) {
            ScreenView m_screen = FindScreen(_screenToHide);
            m_screen.Hide(_direction);
        }

        public void OpenPreviousScreen(Action _tempAction = null) {
            ShowThisScreen(_previousScreen, EnableDirection.Reverse, _tempAction);
        }

        public void HideCurrentScreen(Action _tempAction = null) {
            HideThisScreen(_currentScreen, EnableDirection.Reverse, _tempAction);
        }

        ScreenView FindScreen(ScreenType _type) {
            return _allScreens.Find(x => (x._type == _type))._screen;
        }

        public bool isScreenActive(ScreenType m_type) {
            return _allScreens.Find(x => (x._type == m_type))._screen.isCanvasActive();
        }

        void OnTaskCompleted()
        {
            UIController.instance.HideThisScreen(ScreenType.ACTIVE, EnableDirection.Forward);
            UIController.instance.ShowThisScreen(ScreenType.END, EnableDirection.Forward);
            Events.ChangeSimulationState(SimulationState.UI);
        }
    }

    [Serializable]
    public struct ScreenCollection {
        public ScreenView _screen;
        public ScreenType _type;
    }
}
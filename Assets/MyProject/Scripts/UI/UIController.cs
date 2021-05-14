using System;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo.UI {

    /// <summary>
    /// This class will manage all the screens involved in the scene
    /// </summary>
    public class UIController : MonoBehaviour {

        #region PUBLIC_VARS
        public static UIController instance;

        [Header("Current")]
        public ScreenType _currentScreen;
        public ScreenType _previousScreen;
        #endregion

        #region PRIVATE_VARS
        [Header("Screens")]
        [SerializeField] List<ScreenCollection> _allScreens = null;
        #endregion

        #region UNITY_CALLBACKS
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
        #endregion

        /// <summary>
        /// This method is used for opening a screen
        /// </summary>
        public void ShowThisScreen(ScreenType _screenToShow, EnableDirection _direction, Action _tempAction = null) {
            _previousScreen = _currentScreen;
            ScreenView m_screen = FindScreen(_screenToShow);
            _currentScreen = _screenToShow;
            m_screen.Show(_direction);
        }

        /// <summary>
        /// This method is used for closing a screen
        /// </summary>
        public void HideThisScreen(ScreenType _screenToHide, EnableDirection _direction, Action _tempAction = null) {
            ScreenView m_screen = FindScreen(_screenToHide);
            m_screen.Hide(_direction);
        }

        /// <summary>
        /// This method is used for opening the previous screen
        /// </summary>
        public void OpenPreviousScreen(Action _tempAction = null) {
            ShowThisScreen(_previousScreen, EnableDirection.Reverse, _tempAction);
        }

        /// <summary>
        /// This method is used for closing currently active screen
        /// </summary>
        public void HideCurrentScreen(Action _tempAction = null) {
            HideThisScreen(_currentScreen, EnableDirection.Reverse, _tempAction);
        }

        /// <summary>
        /// This method is used for getting a ScreenView object for any screen 
        /// </summary>
        ScreenView FindScreen(ScreenType _type) {
            return _allScreens.Find(x => (x._type == _type))._screen;
        }

        /// <summary>
        /// This method is used for checking if screen is active ot not
        /// </summary>
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

    /// <summary>
    ///This is the base structure for handling screens
    /// </summary>
    [Serializable]
    public struct ScreenCollection {
        public ScreenView _screen;
        public ScreenType _type;
    }
}
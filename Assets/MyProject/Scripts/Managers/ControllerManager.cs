using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using CommanTickManager;

namespace TrasnferVR.Demo
{
    [DefaultExecutionOrder(kControllerManagerUpdateOrder)]
    public class ControllerManager : MonoBehaviour, ITick
    {
        public static ControllerManager instance;
        public List<ControllerRefs> controllerLeftRefs;
        public List<ControllerRefs> controllerRightRefs;
        [HideInInspector] private ControllerRefs currentLeftHandState;
        [HideInInspector] private ControllerRefs currentRightHandState;
        [SerializeField] private ControllerStates leftHandNormalState;
        [SerializeField] private ControllerStates leftSecondaryState;
        [SerializeField] private ControllerStates rightHandNormalState;
        [SerializeField] private ControllerStates rightHandSecondaryState;

        // Slightly after the default, so that any actions such as release or grab can be processed *before* we switch controllers.
        public const int kControllerManagerUpdateOrder = 10;

        [SerializeField]
        [Tooltip("The buttons on the controller that will trigger a transition to the Teleport Controller.")]
        List<InputHelpers.Button> m_ActivationButtons = new List<InputHelpers.Button>();
        /// <summary>
        /// The buttons on the controller that will trigger a transition to the Teleport Controller.
        /// </summary>
        public List<InputHelpers.Button> activationButtons { get { return m_ActivationButtons; } set { m_ActivationButtons = value; } }


        [SerializeField]
        [Tooltip("The buttons on the controller that will force a deactivation of the teleport option.")]
        List<InputHelpers.Button> m_DeactivationButtons = new List<InputHelpers.Button>();
        /// <summary>
        /// The buttons on the controller that will trigger a transition to the Teleport Controller.
        /// </summary>
        public List<InputHelpers.Button> deactivationButtons { get { return m_DeactivationButtons; } set { m_DeactivationButtons = value; } }

        bool leftSecondaryInputDeactivated = false;
        bool rightSecondaryInputDeactivated = false;

        /// <summary>
        /// A simple state machine which manages the three pieces of content that are used to represent
        /// A controller state within the XR Interaction Toolkit
        /// </summary>
        public class InteractorController
        {
            /// <summary>
            /// The game object that this state controls
            /// </summary>
            public GameObject m_GO;
            /// <summary>
            /// The XR Controller instance that is associated with this state
            /// </summary>
            public XRController m_XRController;
            /// <summary>
            /// The Line renderer that is associated with this state
            /// </summary>
            public XRInteractorLineVisual m_LineRenderer;
            /// <summary>
            /// The interactor instance that is associated with this state
            /// </summary>
            public XRBaseInteractor m_Interactor;

            /// <summary>
            /// When passed a gameObject, this function will scrape the game object for all valid components that we will
            /// interact with by enabling/disabling as the state changes
            /// </summary>
            /// <param name="gameObject">The game object to scrape the various components from</param>
            public void Attach(GameObject gameObject)
            {
                m_GO = gameObject;
                if (m_GO != null)
                {
                    m_XRController = m_GO.GetComponent<XRController>();
                    m_LineRenderer = m_GO.GetComponent<XRInteractorLineVisual>();
                    m_Interactor = m_GO.GetComponent<XRBaseInteractor>();
                    // Debug.Log("xr controller " + (m_XRController!=null) + " for " + m_GO.name);
                    Leave();
                }
            }

            /// <summary>
            /// Enter this state, performs a set of changes to the associated components to enable things
            /// </summary>
            public void Enter()
            {
                if (m_LineRenderer)
                {
                    m_LineRenderer.enabled = true;
                }
                if (m_XRController)
                {
                    m_XRController.enableInputActions = true;
                }
                if (m_Interactor)
                {
                    m_Interactor.enabled = true;
                }
            }

            /// <summary>
            /// Leaves this state, performs a set of changes to the associate components to disable things.
            /// </summary>
            public void Leave()
            {
                if (m_LineRenderer)
                {
                    m_LineRenderer.enabled = false;
                }
                if (m_XRController)
                {
                    m_XRController.enableInputActions = false;
                }
                if (m_Interactor)
                {
                    m_Interactor.enabled = false;
                }
            }
        }

        /// <summary>
        /// The states that we are currently modeling. 
        /// If you want to add more states, add them here!
        /// </summary>
        public enum ControllerStates
        {
            /// <summary>
            /// the Select state is the "normal" ray interaction state for selecting and interacting with objects
            /// </summary>
            Ray = 0,
            /// <summary>
            /// the Teleport state is used to interact with teleport interactors and queue teleportations.
            /// </summary>
            Teleport = 1,
            /// <summary>
            /// the Teleport state is used to interact with teleport interactors and queue teleportations.
            /// </summary>
            Grab = 2,

        }
        [System.Serializable]
        public class ControllerRefs
        {
            public GameObject interactor;
            public ControllerStates state;
            public InteractorController interactionController = new InteractorController();


            public void SetupInteractor()
            {
                interactionController.Attach(interactor);
            }
        }

        private void Awake()
        {
            instance = this;
        }
        void OnEnable()
        {
            leftSecondaryInputDeactivated = false;
            rightSecondaryInputDeactivated = false;

            foreach (ControllerRefs controller in controllerLeftRefs)
            {
                controller.SetupInteractor();
                controller.interactionController.Leave();
            }
            foreach (ControllerRefs controller in controllerRightRefs)
            {
                controller.SetupInteractor();
                controller.interactionController.Leave();
            }

            InputDevices.deviceConnected += RegisterDevices;
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevices(devices);
            for (int i = 0; i < devices.Count; i++)
                RegisterDevices(devices[i]);

            Events.OnSimulationStateChanged += OnSimulationStateChanged;

        }
        void OnDisable()
        {
            InputDevices.deviceConnected -= RegisterDevices;
            Events.OnSimulationStateChanged -= OnSimulationStateChanged;
        }

        void RegisterDevices(InputDevice connectedDevice)
        {
            if (connectedDevice.isValid)
            {
#if UNITY_2019_3_OR_NEWER
                if ((connectedDevice.characteristics & InputDeviceCharacteristics.Left) != 0)
#else
            if (connectedDevice.role == InputDeviceRole.LeftHanded)
#endif
                {
                    ChangeLeftControllerInteraction(ControllerStates.Ray);
                    Debug.Log("Registered Left controller");
                }
#if UNITY_2019_3_OR_NEWER
                else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) != 0)
#else
            else if (connectedDevice.role == InputDeviceRole.RightHanded)
#endif
                {
                    ChangeRightControllerInteraction(ControllerStates.Ray);
                    Debug.Log("Registered Right controller");
                }
            }
        }

        public void Tick()
        {
#if !UNITY_EDITOR
            if (currentLeftHandState != null && currentLeftHandState.interactionController.m_XRController.inputDevice.isValid)
            {
                InputDevice leftInputDevice = currentLeftHandState.interactionController.m_XRController.inputDevice;
                bool activated = false;
                for (int i = 0; i < m_ActivationButtons.Count; i++)
                {
                    leftInputDevice.IsPressed(m_ActivationButtons[i], out bool value);
                    //Debug.Log("Value" + value);
                    activated |= value;
                }

                bool deactivated = false;
                for (int i = 0; i < m_DeactivationButtons.Count; i++)
                {
                    leftInputDevice.IsPressed(m_DeactivationButtons[i], out bool value);
                    deactivated |= value;
                }

                if (deactivated)
                    leftSecondaryInputDeactivated = true;

                // if we're pressing the activation buttons, we transition to Secondary State
                if (activated && !leftSecondaryInputDeactivated)
                {
                    ChangeLeftControllerInteraction(leftSecondaryState);
                }
                // otherwise we're in normal state. 
                else
                {
                    ChangeLeftControllerInteraction(leftHandNormalState);

                    if (!activated)
                        leftSecondaryInputDeactivated = false;
                }
            }

            if (currentRightHandState != null && currentRightHandState.interactionController.m_XRController.inputDevice.isValid)
            {
                InputDevice rightInputDevice = currentRightHandState.interactionController.m_XRController.inputDevice;
                bool activated = false;
                for (int i = 0; i < m_ActivationButtons.Count; i++)
                {
                    rightInputDevice.IsPressed(m_ActivationButtons[i], out bool value);
                    activated |= value;
                }

                bool deactivated = false;
                for (int i = 0; i < m_DeactivationButtons.Count; i++)
                {
                    rightInputDevice.IsPressed(m_DeactivationButtons[i], out bool value);
                    deactivated |= value;
                }

                if (deactivated)
                    rightSecondaryInputDeactivated = true;

                if (activated && !rightSecondaryInputDeactivated)
                {
                    ChangeRightControllerInteraction(rightHandSecondaryState);
                }
                else
                {
                    ChangeRightControllerInteraction(rightHandNormalState);

                    if (!activated)
                        rightSecondaryInputDeactivated = false;
                }
            }
#endif
        }

        public XRController GetLeftController()
        {
            if(currentLeftHandState!=null)
                return currentLeftHandState.interactionController.m_XRController;
            return null;
        }
        public void ChangeLeftControllerInteraction(ControllerStates state)
        {
            if (currentLeftHandState != null)
            {
                if (currentLeftHandState.state == state)
                    return;
                currentLeftHandState.interactionController.Leave();
            }

            ControllerRefs activeState = controllerLeftRefs.Find(x => x.state == state);
            activeState.interactionController.Enter();
            currentLeftHandState = activeState;
        }
        public void ChangeRightControllerInteraction(ControllerStates state)
        {
            if (currentRightHandState != null)
            {
                if (currentRightHandState.state == state)
                    return;
                currentRightHandState.interactionController.Leave();
            }

            ControllerRefs activeState = controllerRightRefs.Find(x => x.state == state);
            activeState.interactionController.Enter();
            currentRightHandState = activeState;
        }

        public void OnSimulationStateChanged(Data.SimulationState state)
        {
            Debug.Log("State Changed to " + state);
            if (state != Data.SimulationState.SIMULATION)
            {
                ProcessingUpdate.Instance.Remove(this);
                ChangeLeftControllerInteraction(ControllerStates.Ray);
                ChangeRightControllerInteraction(ControllerStates.Ray);
            }
            else
            {
                ProcessingUpdate.Instance.Add(this);
            }
        }
    }
}
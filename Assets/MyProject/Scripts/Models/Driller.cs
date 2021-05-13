using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CommanTickManager;

namespace TrasnferVR.Demo
{
    public class Driller : MonoBehaviour, IGrabbable, ITick
    {
        #region PUBLIC_VARS
        //Driller Parameters
        public float rotationSpeed;
        public float pushForce;
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private Transform hoseAttachParent;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Hose hose;
        private Screw screw;
        private bool isDrilling = false;
        private XRController currentHoldedController;
        #endregion

        #region UNITY_CALLBACKS
        private void OnEnable()
        {
            Events.OnResetEnvironment += OnResetEnvrironment;
        }
        private void OnDisable()
        {
            Events.OnResetEnvironment -= OnResetEnvrironment;
        }
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }
        #endregion

        #region PUBLIC_METHODS
        public void Tick()
        {
            if (currentHoldedController != null && currentHoldedController.inputDevice.isValid)
            {
                currentHoldedController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool value);
                if (isDrilling != value)
                {
                    hose.AnimateHose(value);
                }
                isDrilling = value;
            }

            if (isDrilling && screw != null)
            {
                screw.PushScrew(rotationSpeed, pushForce);
            }
        }
        public void OnGrab(XRBaseInteractor interactor)
        {
            currentHoldedController = interactor.GetComponent<XRController>();
            if (hose != null)
            {
                ProcessingUpdate.Instance.Add(this);
            }
        }
        public void OnReleased(XRBaseInteractor interactor)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ProcessingUpdate.Instance.Remove(this);
            currentHoldedController = null;
        }
        public void AttachHose(Hose hose)
        {
            this.hose = hose;
            hose.transform.parent = hoseAttachParent;
            hose.transform.localPosition = Vector3.zero;
            hose.transform.localRotation = Quaternion.identity;

            if (currentHoldedController != null)
            {
                ProcessingUpdate.Instance.Add(this);
            }
        }
        public void ScrewConnected(Screw screw)
        {
            this.screw = screw;
        }
        public void ScrewDisconnect()
        {
            screw = null;
        }
        #endregion

        #region PRIVATE_METHODS
        void OnResetEnvrironment()
        {
            hose = null;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ProcessingUpdate.Instance.Remove(this);

            currentHoldedController = null;
            hose.AnimateHose(false);
        }
        #endregion
    }
}

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
        public List<HighlightObjectData> highlightObjectData;
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private Transform hoseAttachParent;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Transform drillerParent;
        private Hose hose;
        private Screw screw;
        private bool isDrilling = false;
        private XRController currentHoldedController;
        private XRBaseInteractor xRBaseInteractor;
        #endregion

        #region UNITY_CALLBACKS
        private void OnEnable()
        {
            Events.OnResetEnvironment += OnResetEnvrironment;
            Events.OnTaskCompleted += OnTaskCompleted;
        }
        private void OnDisable()
        {
            Events.OnResetEnvironment -= OnResetEnvrironment;
            Events.OnTaskCompleted -= OnTaskCompleted;
        }
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            drillerParent = transform.parent;
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
        public void OnHoverEnter()
        {
            foreach (HighlightObjectData data in highlightObjectData)
            {
                data.objectRenderer.material = data.highlightedMaterial;
            }
        }
        public void OnHoverExit()
        {
            foreach (HighlightObjectData data in highlightObjectData)
            {
                data.objectRenderer.material = data.normalMaterial;
            }
        }
        public void OnGrab(XRBaseInteractor interactor)
        {
            xRBaseInteractor = interactor;
            currentHoldedController = interactor.GetComponent<XRController>();
            OnHoverExit();
            if (hose != null)
            {
                ProcessingUpdate.Instance.Add(this);
            }
        }
        public void OnReleased(XRBaseInteractor interactor)
        {
            xRBaseInteractor = null;
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

            foreach (HighlightObjectData data in hose.highlightObjectData)
            {
                highlightObjectData.Add(data);
            }

            if (currentHoldedController != null)
            {
                ProcessingUpdate.Instance.Add(this);
            }

            Events.HoseAttached();
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
            if (hose != null)
            {
                foreach (HighlightObjectData data in hose.highlightObjectData)
                {
                    highlightObjectData.Remove(data);
                }
                hose.AnimateHose(false);
                hose = null;
            }
            isDrilling = false;
            transform.parent = drillerParent;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ProcessingUpdate.Instance.Remove(this);

            currentHoldedController = null;
        }
        void OnTaskCompleted()
        {
            if (hose != null)
            {
                hose.AnimateHose(false);
            }

            ControllerManager.instance.FixForceDropGrabLeft();
            ControllerManager.instance.FixForceDropGrabRight();

            this.Execute(() =>
            {
                transform.parent = drillerParent;
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                OnHoverExit();
                // currentHoldedController = null;
            }, 0.2f);

            isDrilling = false;
            transform.parent = drillerParent;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ProcessingUpdate.Instance.Remove(this);
            currentHoldedController = null;
        }
        #endregion
    }
}

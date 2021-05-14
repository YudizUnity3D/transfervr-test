using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CommanTickManager;

namespace TrasnferVR.Demo
{
    /// <summary>
    /// This class will manage all the interaction for Driller Object
    /// </summary>
    public class Driller : MonoBehaviour, IGrabbable, ITick
    {
        #region PUBLIC_VARS
        //Driller Parameters
        public float rotationSpeed;
        public float pushForce;
        public List<HighlightObjectData> highlightObjectData;

        #endregion

        #region PRIVATE_VARS
        [SerializeField] private XRGrabInteractable grabInteractable;
        [SerializeField] private Transform hoseAttachParent;
        [SerializeField] private AudioSource audioSource;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Transform drillerParent;
        private Hose hose;
        private Screw screw;
        private bool isDrilling = false;
        private XRController currentHoldedController;
        private XRBaseInteractor xRBaseInteractor;
        private LayerMask interactionLayerMask;
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
            interactionLayerMask = grabInteractable.interactionLayerMask;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            drillerParent = transform.parent;
        }
        #endregion

        #region PUBLIC_METHODS
        /// <summary>
        /// This is called in update function if adding using Processing Update. This manages the drilling mechanism and animations
        /// </summary>
        public void Tick()
        {
            if (currentHoldedController != null && currentHoldedController.inputDevice.isValid)
            {
                currentHoldedController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool value);
                if (isDrilling != value)
                {
                    hose.AnimateHose(value);
                    if (value)
                        PlayAudio(AudioType.DRILL, true);
                    else
                        StopAudio();
                }
                isDrilling = value;
            }

            if (isDrilling && screw != null)
            {
                screw.PushScrew(rotationSpeed, pushForce);
            }
        }
        /// <summary>
        /// Showing highlight effect when near the controller
        /// </summary>
        public void OnHoverEnter()
        {
            foreach (HighlightObjectData data in highlightObjectData)
            {
                data.objectRenderer.material = data.highlightedMaterial;
            }
        }
        /// <summary>
        /// Hide highlight effect when near the controller
        /// </summary>
        public void OnHoverExit()
        {
            foreach (HighlightObjectData data in highlightObjectData)
            {
                data.objectRenderer.material = data.normalMaterial;
            }
        }
        /// <summary>
        /// Manages data when driller grabbed
        /// </summary>
        public void OnGrab(XRBaseInteractor interactor)
        {
            Debug.Log("OnGrab  " + interactor.gameObject.name);
            xRBaseInteractor = interactor;
            currentHoldedController = interactor.GetComponent<XRController>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            // OnHoverExit();
            if (hose != null)
            {
                ProcessingUpdate.Instance.Add(this);
            }
        }
        /// <summary>
        /// Manages data and positioning when driller released 
        /// </summary>
        public void OnReleased(XRBaseInteractor interactor)
        {
            Debug.Log("OnRelease  " + interactor.gameObject.name);
            // grabInteractable.ForceHoverExit(interactor);
            xRBaseInteractor = null;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ProcessingUpdate.Instance.Remove(this);
            currentHoldedController = null;
        }
        /// <summary>
        /// This will attach hose to the driller
        /// </summary>
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

            PlayAudio(AudioType.METALATTACH, false);
            Events.HoseAttached();
        }
        /// <summary>
        /// This will let driller know that screw is connected and can drill 
        /// </summary>
        public void ScrewConnected(Screw screw)
        {
            this.screw = screw;
            screw.PlayParticle(true);
        }
        /// <summary>
        /// This will stop pushing the screw inside as it disconnects
        /// </summary>
        public void ScrewDisconnect()
        {
            if (screw != null)
                screw.PlayParticle(false);
            screw = null;
        }
        #endregion

        #region PRIVATE_METHODS
        /// <summary>
        /// Manages sounds played by driller
        /// </summary>
        void PlayAudio(AudioType type, bool isLooping)
        {
            AudioClip clip = SoundManager.instance.GetAudioClip(type);
            audioSource.loop = isLooping;
            audioSource.clip = clip;
            audioSource.Play();
        }
        void StopAudio()
        {
            audioSource.Stop();
        }
        /// <summary>
        /// This will reset the driller data and position
        /// </summary>
        void OnResetEnvrironment()
        {
            if (hose != null)
            {
                foreach (HighlightObjectData data in hose.highlightObjectData)
                {
                    highlightObjectData.Remove(data);
                }
                hose.AnimateHose(false);
                StopAudio();
                hose = null;
            }
            grabInteractable.interactionLayerMask = interactionLayerMask;
            isDrilling = false;
            transform.parent = drillerParent;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ProcessingUpdate.Instance.Remove(this);

            currentHoldedController = null;
        }
        /// <summary>
        /// This will reset position and remove grabbing after task completion
        /// </summary>
        void OnTaskCompleted()
        {
            if (hose != null)
            {
                hose.AnimateHose(false);
                StopAudio();
            }

            ControllerManager.instance.FixForceDropGrabLeft();
            ControllerManager.instance.FixForceDropGrabRight();
            // grabInteractable.CustomForceDrop(xRBaseInteractor);
            grabInteractable.interactionLayerMask = 0;

            this.Execute(() =>
            {
                transform.parent = drillerParent;
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                // grabInteractable.interactionLayerMask = interactionLayerMask;
                OnHoverExit();
                // currentHoldedController = null;
            }, 0.3f);

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

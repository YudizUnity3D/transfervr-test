using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TrasnferVR.Demo
{
    /// <summary>
    /// This script contains all the functionality related to Hose
    /// </summary>
    public class Hose : MonoBehaviour, IGrabbable
    {
        #region PUBLIC_VARS
        public List<HighlightObjectData> highlightObjectData;
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private Animator hoseAnimator;
        [SerializeField] private XRGrabInteractable xRGrabInteractable;
        [SerializeField] private LayerMask grabLayerMask;
        [SerializeField] private Collider grabCollider;
        [SerializeField] private Collider drillingCollider;
        private Driller attachedDriller;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private bool isAttachedAnywhere;
        private GameObject connectedObject;
        private Transform initialParent;
        private bool isHoseRotating = false;
        #endregion

        #region UNITY_CALLBACKS
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialParent = transform.parent;
            ToggleAttachedCollider(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            connectedObject = other.gameObject;
            if (attachedDriller != null)
            {
                Screw screw = connectedObject.GetComponent<Screw>();
                attachedDriller.ScrewConnected(screw);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (attachedDriller != null)
            {
                Screw screw = other.GetComponent<Screw>();
                attachedDriller.ScrewDisconnect();
            }
            connectedObject = null;
        }
        private void OnEnable()
        {
            Events.OnResetEnvironment += OnResetEnvrironment;
        }
        private void OnDisable()
        {
            Events.OnResetEnvironment -= OnResetEnvrironment;
        }
        #endregion

        #region PUBLIC_METHODS
        /// <summary>
        /// This will start or stop rotation of hose while drilling
        /// </summary>
        public void AnimateHose(bool isRotating)
        {
            if (isHoseRotating == isRotating)
                return;
            string triggerType = isRotating ? Constants.startAnimation : Constants.stopAnimation;
            isHoseRotating = isRotating;
            hoseAnimator.SetTrigger(triggerType);

        }
        /// <summary>
        /// This will toggle the collider hose uses for grab or screw drill detection
        /// </summary>
        public void ToggleAttachedCollider(bool isHoseAttached)
        {
            drillingCollider.enabled = isHoseAttached;
            grabCollider.enabled = !isHoseAttached;
        }
        /// <summary>
        /// This will enable or distable grab interaction with Hose
        /// </summary>
        public void ToggleGrabInteraction(bool interact)
        {
            if (interact)
            {
                xRGrabInteractable.interactionLayerMask = grabLayerMask;
            }
            else
            {
                xRGrabInteractable.interactionLayerMask = 0;
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
        /// This will manage data when grabbed 
        /// </summary>
        [ContextMenu("Grab")]
        public void OnGrab(XRBaseInteractor interactor)
        {
            isAttachedAnywhere = false;
            // OnHoverExit();
        }
        /// <summary>
        /// This will manage data and positioning when released 
        /// </summary>
        [ContextMenu("Release")]
        public void OnReleased(XRBaseInteractor interactor)
        {
            if (connectedObject != null)
            {
                Driller driller = connectedObject.GetComponentInParent<Driller>();
                if (driller != null)
                {
                    attachedDriller = driller;
                    driller.AttachHose(this);
                    this.Execute(() =>
                    {
                        ToggleAttachedCollider(true);
                        ToggleGrabInteraction(false);
                    }, 0.1f);
                    isAttachedAnywhere = true;
                }
            }
            if (!isAttachedAnywhere)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                isAttachedAnywhere = false;
            }
        }
        /// <summary>
        /// This will reset the data and positioning 
        /// </summary>
        void OnResetEnvrironment()
        {
            transform.parent = initialParent;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            OnHoverExit();

            this.Execute(() =>
                    {
                        ToggleAttachedCollider(false);
                        ToggleGrabInteraction(true);
                    }, 0.1f);
        }
        #endregion
    }
}
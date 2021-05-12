using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TrasnferVR.Demo
{
    public class Hose : MonoBehaviour, IGrabbable
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private XRGrabInteractable xRGrabInteractable;
        [SerializeField] private LayerMask grabLayerMask;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private bool isAttachedAnywhere;
        private GameObject connectedObject;
        private Transform initialParent;
        #endregion

        #region UNITY_CALLBACKS
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialParent = transform.parent;
        }
        private void OnTriggerEnter(Collider other)
        {
            connectedObject = other.gameObject;
        }
        private void OnTriggerExit(Collider other)
        {
            connectedObject = null;
        }
        private void OnEnable()
        {
            Events.OnResetEnvironment+=OnResetEnvrironment;
        }
        private void OnDisable()
        {
            Events.OnResetEnvironment-=OnResetEnvrironment;
        }
        #endregion

        #region PUBLIC_METHODS
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
        [ContextMenu("Grab")]
        public void OnGrab()
        {
            isAttachedAnywhere = false;
        }
        [ContextMenu("Release")]
        public void OnReleased()
        {
            if (connectedObject != null)
            {
                Driller driller = connectedObject.GetComponentInParent<Driller>();
                if (driller != null)
                {
                    driller.AttachHose(this);
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
        void OnResetEnvrironment()
        {
            transform.parent = initialParent;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        #endregion
    }
}
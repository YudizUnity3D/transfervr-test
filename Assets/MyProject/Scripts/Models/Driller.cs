using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrasnferVR.Demo
{
    public class Driller : MonoBehaviour, IGrabbable
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private Transform hoseAttachParent;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Hose hose;
        #endregion

        #region PUBLIC_METHODS
        public void OnGrab()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }
        public void OnReleased()
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        public void AttachHose(Hose hose)
        {
            this.hose = hose;
            hose.transform.parent = hoseAttachParent;
            hose.transform.localPosition = Vector3.zero;
            hose.transform.localRotation = Quaternion.identity;
        }
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrasnferVR.Demo
{
    public class Screw : MonoBehaviour
    {

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }
        private void OnEnable()
        {
            Events.OnResetEnvironment += OnResetEnvrironment;
        }
        private void OnDisable()
        {
            Events.OnResetEnvironment -= OnResetEnvrironment;
        }
        
        public void PushScrew(float rotationSpeed, float force)
        {
            transform.Translate(-transform.up * force * Time.deltaTime);
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(Constants.drillableObject))
            {
                // Task Complete
                Debug.Log("Task Completed");
                Events.TaskCompleted();
            }
        }

        void OnResetEnvrironment()
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}
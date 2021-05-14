using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrasnferVR.Demo
{
    /// <summary>
    /// This script contains all functionality related to screw 
    /// </summary>
    public class Screw : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
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

        /// <summary>
        /// This method will push the screw towards the bench wood and rotate it 
        /// </summary>
        public void PushScrew(float rotationSpeed, float force)
        {
            transform.Translate(-transform.up * force * Time.deltaTime);
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
        }
        public void PlayParticle(bool play)
        {
            if(play)
                particleSystem.Play();
            else
                particleSystem.Stop();
        }
        /// <summary>
        /// This manages detection of complete insertion and task completion 
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(Constants.drillableObject))
            {
                // Task Complete
                Debug.Log("Task Completed");
                Events.TaskCompleted();
            }
        }
        /// <summary>
        /// This resets position of screw to before drill 
        /// </summary>
        void OnResetEnvrironment()
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}
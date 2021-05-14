using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrasnferVR.Demo {

    /// <summary>
    /// This class is a data and enums container for all the globally entities
    /// </summary>
    public class Data : MonoBehaviour {

        /// <summary>
        /// This is an enum for all the screen types
        /// </summary>
        public enum ScreenType {

            START,
            INSTRUCTIONS,
            ACTIVE,
            PAUSE,
            END
        }

        /// <summary>
        /// This enum contains the state of simulation
        /// </summary>
        public enum SimulationState {

            UI,
            SIMULATION,

        }

        /// <summary>
        /// This enum contains the instruction types of simulation
        /// </summary>
        public enum InstuctionType {

            NONE,
            SETUP_EQUIPMENTS,
            DRILLING

        }

    }
}
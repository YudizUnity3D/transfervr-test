using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrasnferVR.Demo {
    public class Data : MonoBehaviour {

        public enum ScreenType {

            START,
            INSTRUCTIONS,
            ACTIVE,
            PAUSE,
            END
        }

        public enum SimulationState {

            UI,
            SIMULATION,

        }

    }
}
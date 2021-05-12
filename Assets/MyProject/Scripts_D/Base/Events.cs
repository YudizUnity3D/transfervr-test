using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrasnferVR.Demo.Data;

namespace TrasnferVR.Demo {
    public class Events : MonoBehaviour {

        public static event Action<SimulationState> OnSimulationStateChanged = delegate { };

        public static void ChangeSimulationState(SimulationState simulationState) {
            OnSimulationStateChanged(simulationState);
        }

    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRQualitySettings : MonoBehaviour
{
    private void Awake()
    {
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 1.3f;
    }
}

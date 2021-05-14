using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class CustomGrabInteractable : XRGrabInteractable
{
    public void CustomForceDrop(XRBaseInteractor interactor)
    {
        OnSelectExit(interactor);
    }

    public void ForceHoverExit(XRBaseInteractor interactor)
    {
        OnHoverExit(interactor);
    }
}

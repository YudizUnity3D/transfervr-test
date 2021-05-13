using UnityEngine.XR.Interaction.Toolkit;
public interface IGrabbable
{
    void OnGrab(XRBaseInteractor interactor);
    void OnReleased(XRBaseInteractor interactor);
}

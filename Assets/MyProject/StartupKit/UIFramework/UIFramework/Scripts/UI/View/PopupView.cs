using UnityEngine;


public class PopupView : ScreenView
{
    public void OnPopupClose()
    {
        PopupController.instance.CloseLastOpened();
    }
}
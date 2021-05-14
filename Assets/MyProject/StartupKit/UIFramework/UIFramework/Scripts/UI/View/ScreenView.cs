using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TrasnferVR.Demo;

public class ScreenView : UIBase
{
    public delegate void CanvasShowHideCalls(bool status);

    public event CanvasShowHideCalls OnCanvasShowHideCalled;


    public override void Show(EnableDirection m_direction)
    {
        _raycaster.enabled = true;
        if (OnCanvasShowHideCalled != null)
            OnCanvasShowHideCalled(true);
        base.ShowCanvas(m_direction);
    }

    public override void Hide(EnableDirection m_direction)
    {
        _raycaster.enabled = false;
        if (OnCanvasShowHideCalled != null)
            OnCanvasShowHideCalled(false);
        base.HideCanvas(m_direction);
    }


    public override void OnScreenLoaded()
    {
        StartCoroutine("CheckForBackKey");
    }

    public override void OnScreenHidden()
    {
        StopCoroutine("CheckForBackKey");
    }

    public override void OnBackKeyPressed()
    {
    }

    public bool isCanvasActive()
    {
        return _canvas.enabled;
    }


    IEnumerator CheckForBackKey()
    {
        bool isMenuPressed = false;
        while (true)
        {
#if !UNITY_EDITOR
            XRController leftController = ControllerManager.instance.GetLeftController();
            if (leftController != null && leftController.inputDevice.isValid)
            {
                leftController.inputDevice.IsPressed(InputHelpers.Button.MenuButton, out bool value);
                if (value && !isMenuPressed)
                {
                if(isCanvasActive())
                {
                    OnBackKeyPressed();
                    isMenuPressed = true;
                }
                else
                {
                    StopCoroutine("CheckForBackKey");
                }
                } 
            }
#elif UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isCanvasActive())
                {
                    OnBackKeyPressed();
                }
                else
                {
                    StopCoroutine("CheckForBackKey");
                }
            }
#endif
            yield return null;
        }
    }
}
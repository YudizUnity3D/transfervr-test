using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SimpleAnimation : MonoBehaviour, IBaseCanvasStateListner
{
    public bool shouldLookForBaseUI = true;
    public bool shouldAnimateOnBecameVisible = false;
    [SerializeField] bool shouldLookForContent = false;
    bool hasCanvas;
    public float showAnimationTime = 0.8f;
    public Ease showAnimationEffect = Ease.OutExpo;
    public float hideAnimationTime = 0.5f;
    public Ease hideAnimationEffect = Ease.InExpo;
    public UnityEvent OnShowAnimationFinish;
    public UnityEvent OnHideAnimationFinish;
    bool hasAnimationComponent;
    BaseAnimation _animationComponent;
    UIBase _currentUIBase;
    Canvas _canvas;
    Transform _targetTransform;


    [Header("Has to Enable/Disable Content After Screen Is Hidden ?")]
    public bool HasToEnableDisable = true;


    private void Awake()
    {
        if (shouldAnimateOnBecameVisible)
        {
            if (GetComponentInParent<ScreenView>())
            {
                GetComponentInParent<ScreenView>().OnCanvasShowHideCalled += OnCanvasEnableCalled;

            }
            shouldLookForBaseUI = false;
        }
        if (!hasAnimationComponent)
            Initialize();
    }

    private void OnCanvasEnableCalled(bool status)
    {
        StartShowAnimation(EnableDirection.Forward);
        //Debug.Log("EnableCalled " + status);
    }

    async void Initialize()
    {
        if (GetComponent<BaseAnimation>())
        {
            if (GetComponent<Canvas>())
            {
                _canvas = GetComponent<Canvas>();
                hasCanvas = true;
            }
            if (shouldLookForContent && !shouldLookForBaseUI)
            {
                _targetTransform = transform.Find("Content");
                if (_targetTransform == null)
                    _targetTransform = transform;

            }
            else
                _targetTransform = transform;
            _animationComponent = GetComponent<BaseAnimation>();
            _animationComponent.SetupAnimation(_targetTransform);
            hasAnimationComponent = true;
            if (shouldLookForBaseUI)
                SubscribeToCanvasEvents();
        }

        await Task.Delay(200);

        if (HasToEnableDisable)
        {
            if (_targetTransform != null)
            {
                _targetTransform.gameObject.SetActive(false);
            }
        }

    }

    public void SubscribeToCanvasEvents()
    {
        if (GetComponentInParent<UIBase>())
        {
            _currentUIBase = GetComponentInParent<UIBase>();
            _currentUIBase.OnPanelStateChanged += OnNotify;
        }
    }

    private void OnDestroy()
    {
        if (GetComponent<BaseAnimation>())
        {
            UnSubscribeToCanvasEvents();
        }
    }

    public void StartShowAnimation(EnableDirection _direction)
    {
        if (HasToEnableDisable)
        {
            _targetTransform.gameObject.SetActive(true);
        }
        if (!hasAnimationComponent)
            Initialize();
        if (hasAnimationComponent)
        {
            if (hasCanvas)
                _canvas.enabled = true;
            _animationComponent.ShowAnimation(_direction, showAnimationTime, showAnimationEffect, ShowAnimationComplete);
            if (_currentUIBase)
            {
                _currentUIBase.OnElementShowAnimationStarted(this);
            }
            else
            {
                if (!hasCanvas)
                    gameObject.SetActive(true);
            }
        }
    }

    public void StartHideAnimation(EnableDirection _direction)
    {

        if (!hasAnimationComponent)
            Initialize();
        if (hasAnimationComponent)
        {
            _animationComponent.HideAnimation(_direction, hideAnimationTime, hideAnimationEffect, HideAnimationComplete);
            if (_currentUIBase)
            {
                _currentUIBase.OnElementHideAnimationStarted(this);
            }
        }
    }





    void ShowAnimationComplete()
    {
        if (OnShowAnimationFinish != null)
        {
            OnShowAnimationFinish.Invoke();
        }
        if (_currentUIBase)
        {
            _currentUIBase.OnElementShowAnimationFinished(this);
        }

    }

    async void HideAnimationComplete()
    {
        if (OnHideAnimationFinish != null)
        {
            OnHideAnimationFinish.Invoke();
        }
        if (hasCanvas)
            _canvas.enabled = false;
        if (_currentUIBase)
        {
            _currentUIBase.OnElementHideAnimationFinished(this);
        }
        else
        {
            if (!hasCanvas)
                gameObject.SetActive(false);
        }

        await Task.Delay(100);

        if (HasToEnableDisable)
        {
            _targetTransform.gameObject.SetActive(false);
        }

    }



    public void UnSubscribeToCanvasEvents()
    {
        if (_currentUIBase)
        {
            _currentUIBase.OnPanelStateChanged -= OnNotify;
        }
    }

    public virtual void OnNotify(CanvasState _currentState, EnableDirection _direction)
    {
        if (_currentState == CanvasState.Enabled)
        {
            StartShowAnimation(_direction);
        }
        else
        {
            StartHideAnimation(_direction);
        }
    }

    public void ResetAnimation()
    {
        _animationComponent.ResetAnimation();
    }
}
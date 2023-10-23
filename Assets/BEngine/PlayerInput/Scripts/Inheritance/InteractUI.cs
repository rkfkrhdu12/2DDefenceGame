using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIInputComponent))]
public class InteractUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public virtual void Interact() { }

    public virtual void OnPointerDown(PointerEventData eventData) => OnDown(eventData);
    public virtual void OnPointerUp(PointerEventData eventData) => OnUp(eventData);
    public virtual void OnPointerEnter(PointerEventData eventData) => OnEnter(eventData);
    public virtual void OnPointerExit(PointerEventData eventData) => OnExit(eventData);
    
    #region Variable

    protected InputSystem _inputSystem = null;

    protected UIInputComponent _uiInputComponent = null;

    protected bool CurrentUIMode => _inputSystem.UIMode;

    protected bool IsAlwaysInputActive => _uiInputComponent.IsAlwaysInputActive;

    #endregion

    #region Private Function

    void OnDown(PointerEventData eventData) { if (IsInputSuccess(eventData)) _uiInputComponent.OnDown(); }
    void OnUp(PointerEventData eventData) { if (IsInputSuccess(eventData)) _uiInputComponent.OnUp(); }
    void OnEnter(PointerEventData eventData) { if (IsInputSuccess(eventData)) _uiInputComponent.OnEnter(); }
    void OnExit(PointerEventData eventData) { if (IsInputSuccess(eventData)) _uiInputComponent.OnExit(); }

    protected bool IsInputSuccess(PointerEventData eventData)
    {
        if (!CurrentUIMode || IsAlwaysInputActive) return false;
        if (_uiInputComponent == null) return false;
        if (!_uiInputComponent.IsMouseMode) return false;

        int inputType = 1 << (int)eventData.button;
        int selectType = (int)_uiInputComponent.MouseInputType;

        return (selectType & inputType) == inputType;
    }

    #endregion

    #region Unity Function
    protected void Awake()
    {
        _uiInputComponent = GetComponent<UIInputComponent>();
    }

    protected void Start()
    {
        _inputSystem = InputSystem.Instance;
    } 
    #endregion
}

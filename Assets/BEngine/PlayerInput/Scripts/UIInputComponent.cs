using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIInputComponent : MonoBehaviour
{
    public void OnDown(bool value = false) => _inputEvent?.OnDown?.Invoke();
    public void OnPress(bool value = false) => _inputEvent?.OnPress?.Invoke();
    public void OnUp(bool value = false) => _inputEvent?.OnUp?.Invoke();
    public void OnEnter(bool value = false) => _inputEvent?.OnEnter?.Invoke();
    public void OnExit(bool value = false) => _inputEvent?.OnExit?.Invoke();

    public EInputType InputType { get => _inputType; set => _inputType = value; }
    public EUIInputKeyType KeyboardInputType { get => _keyboardInputType; set => _keyboardInputType = value; }
    public EUIInputMouseType MouseInputType { get => _mouseInputType; set => _mouseInputType = value; }
    public bool IsKeyboardMode { get => _inputType == EInputType.Keyboard; }
    public bool IsMouseMode { get => _inputType == EInputType.Mouse; }
    public bool IsAlwaysInputActive { get => _isAlwaysInputActive; set => _isAlwaysInputActive = value; }

    public EInputType PrevInputType { get => _prevInputType; set => _prevInputType = value; }
    public EUIInputKeyType PrevKeyboardInputType { get => _prevKeyboardInputType; set => _prevKeyboardInputType = value; }
    public EUIInputMouseType PrevMouseInputType { get => _prevMouseInputType; set => _prevMouseInputType = value; }

    #region Private Variable
    [SerializeField, RealTimeUpdate("UpdateType")]
    private EInputType _inputType;

    [HideInInspector, SerializeField, EnumFlags]
    private EUIInputKeyType _keyboardInputType;
    [HideInInspector, SerializeField, EnumFlags]
    private EUIInputMouseType _mouseInputType;
    [HideInInspector, SerializeField, RealTimeUpdate("UpdateEvent")]
    private bool _isAlwaysInputActive = false;

    [HideInInspector, SerializeField]
    private UIInputEvent _inputEvent;

    private EInputType _prevInputType;
    private EUIInputKeyType _prevKeyboardInputType;
    private EUIInputMouseType _prevMouseInputType;

    [SerializeField]
    private InputUIManager _inputUIManager = null;

    #endregion

    #region Private Function

    void UpdateType() => _inputUIManager?.UpdateType(this);
    void UpdateEvent() => _inputUIManager?.UpdateEvent(this);

    #endregion

    #region Unity Function

    private void Awake()
    {
        _prevInputType = EInputType.None;

        _inputUIManager = InputSystem.Instance.InputUIMgr;
    }

    private void Start()
    {
        UpdateType();
    }
    #endregion
}

#region Define

[System.Serializable]
public class UIInputEvent
{
    public UnityEngine.Events.UnityEvent OnEnter;
    public UnityEngine.Events.UnityEvent OnExit;

    public UnityEngine.Events.UnityEvent OnDown;
    public UnityEngine.Events.UnityEvent OnPress;
    public UnityEngine.Events.UnityEvent OnUp;
}


#if UNITY_EDITOR

// Union Inspector

[CustomEditor(typeof(UIInputComponent))]
public class InputTypeInspectorEditor : Editor
{
    private readonly string InputTypePropertyName = "_inputType";
    private readonly string InputEventPropertyName = "_inputEvent";

    private readonly string KeyboardTypePropertyName = "_keyboardInputType";
    private readonly string MouseTypePropertyName = "_mouseInputType";

    private readonly string IsAlwaysInputActivePropertyName = "_isAlwaysInputActive";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        var inputType = (EInputType)serializedObject.FindProperty(InputTypePropertyName).intValue;

        EditorGUILayout.PropertyField(inputType == EInputType.Mouse ?
            serializedObject.FindProperty(MouseTypePropertyName) :
            serializedObject.FindProperty(KeyboardTypePropertyName));

        if(inputType == EInputType.Mouse)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(IsAlwaysInputActivePropertyName));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty(InputEventPropertyName + ".OnDown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(InputEventPropertyName + ".OnPress"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(InputEventPropertyName + ".OnUp"));

        if (inputType == EInputType.Mouse)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(InputEventPropertyName + ".OnEnter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(InputEventPropertyName + ".OnExit"));
        }

        // 변경된 프로퍼티를 저장해줍니다.
        serializedObject.ApplyModifiedProperties();
    }
}

#endif
#endregion
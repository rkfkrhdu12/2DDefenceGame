using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInputType
{
    None,
    Keyboard,
    Mouse,
}

public enum EUIInputKeyType
{
    Enter = 1,
    Cancle = 1 << 1,

    Next = 1 << 2,
    Prev = 1 << 3,
}

public enum EUIInputMouseType
{
    MouseClick = 1,
    MouseRightClick = 1 << 1,
    MouseMiddleClick = 1 << 2,
}

public class InputUIManager
{
    public void UpdateType(UIInputComponent uIInputComponent)
    {
        if (uIInputComponent == null) return;

        var currentInputType = uIInputComponent.InputType;
        if (currentInputType == uIInputComponent.PrevInputType) return;
        uIInputComponent.PrevInputType = currentInputType;

        switch (currentInputType)
        {
            case EInputType.Keyboard:
                UpdateEvent(uIInputComponent, typeof(EUIInputMouseType), 0, (int)uIInputComponent.PrevMouseInputType);

                uIInputComponent.PrevKeyboardInputType = 0;

                UpdateEvent(uIInputComponent);
                break;
            case EInputType.Mouse:
                UpdateEvent(uIInputComponent, typeof(EUIInputKeyType), 0, (int)uIInputComponent.PrevKeyboardInputType);

                uIInputComponent.PrevMouseInputType = 0;

                if (uIInputComponent.IsAlwaysInputActive) UpdateEvent(uIInputComponent);
                break;
        }
    }

    public void UpdateEvent(UIInputComponent uIInputComponent)
    {
        switch (uIInputComponent.InputType)
        {
            case EInputType.Keyboard:
                if (uIInputComponent.KeyboardInputType == uIInputComponent.PrevKeyboardInputType) return;

                UpdateEvent(uIInputComponent, typeof(EUIInputKeyType), (int)uIInputComponent.KeyboardInputType, (int)uIInputComponent.PrevKeyboardInputType);

                uIInputComponent.PrevKeyboardInputType = uIInputComponent.KeyboardInputType;
                break;
            case EInputType.Mouse:
                EUIInputMouseType currentInputType = uIInputComponent.IsAlwaysInputActive ? uIInputComponent.MouseInputType : 0;
                if (currentInputType == uIInputComponent.PrevMouseInputType) return;

                UpdateEvent(uIInputComponent, typeof(EUIInputMouseType), (int)currentInputType, (int)uIInputComponent.PrevMouseInputType);

                uIInputComponent.PrevMouseInputType = currentInputType;
                break;
        }
    }

    void UpdateEvent(UIInputComponent uIInputComponent, System.Type enumType, int currentData, int prevData)
    {
        if (_keyList.Count == 0) Initialize();
        if (uIInputComponent == null) return;

        string[] uitypeNameList = Enum.GetNames(enumType);
        Array uitypeDataList = Enum.GetValues(enumType);

        for (int i = 0; i < uitypeNameList.Length; ++i)
        {
            if (!_keyList.ContainsKey(uitypeNameList[i])) return;

            int currentFindData = (int)uitypeDataList.GetValue(i);
            if (((currentData ^ prevData) & currentFindData) == currentFindData)
            {
                var currentKey = _keyList[uitypeNameList[i]];
                if (!((prevData & currentFindData) == currentFindData))
                {
                    currentKey.OnDown += uIInputComponent.OnDown;
                    currentKey.OnPress += uIInputComponent.OnPress;
                    currentKey.OnUp += uIInputComponent.OnUp;
                }
                else
                {
                    currentKey.OnDown -= uIInputComponent.OnDown;
                    currentKey.OnPress -= uIInputComponent.OnPress;
                    currentKey.OnUp -= uIInputComponent.OnUp;
                }
            }
        }
    }

    readonly Dictionary<string, InputSystem.Key<bool>> _keyList = new();

    void Initialize()
    {
        var inputSystem = InputSystem.Instance;
        if (inputSystem == null) return;

        var keytypeNameList = System.Enum.GetNames(typeof(EKeyState));
        for (int i = 0; i < (int)EKeyState.Last; ++i)
        {
            _keyList.Add(keytypeNameList[i], inputSystem.GetButton((EKeyState)i));
        }
    }

    void Awake()
    {
        Initialize();
    }

}

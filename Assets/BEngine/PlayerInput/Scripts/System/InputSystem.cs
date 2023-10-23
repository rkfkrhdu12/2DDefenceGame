using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

// Notion Page
// https://www.notion.so/Player-Input-62ef971f73924cb0bb6141e5b42dd840



public enum EKeyState
{
    Move,
    Look,
    MousePoint,
    MouseWheel,

    Enter,
    Cancle,

    MouseClick,
    MouseRightClick,
    MouseMiddleClick,

    Fire,
    // Add New CustomKey
    Next,
    Prev,

    Last
}

public class InputSystem : MonoBehaviour
{
    //* Function *//
    public Key<Vector2> GetAxisButton(EKeyState keyState) => GetAxisButtonA(keyState);
    public Key<bool> GetButton(EKeyState keyState) => GetButtonA(keyState);

    public bool UIMode { get => _isUIMode; set => _isUIMode = value; }

    public InputKey<Vector2> MoveDirection = new();
    public InputKey<Vector2> LookDirection = new();

    public InputKey<bool> EnterButton = new();
    public InputKey<bool> CancleButton = new();

    public InputKey<Vector2> MousePosition = new();
    public InputKey<bool> MouseLeftButton = new();
    public InputKey<bool> MouseMiddleButton = new();
    public InputKey<bool> MouseRightButton = new();
    public InputKey<Vector2> MouseScrollWheel = new();

    public InputKey<bool> Button01 = new();
    public InputKey<bool> NextButton = new();
    public InputKey<bool> PrevButton = new();

    public InputUIManager InputUIMgr = new();

    #region Custom Key
    void OnRegisterKey()
    {
        // Set New CustomKey
        Button01.Set(ref GetKeyRefData(EKeyState.Fire));
        NextButton.Set(ref GetKeyRefData(EKeyState.Next));
        PrevButton.Set(ref GetKeyRefData(EKeyState.Prev));

    }

    // Add New CustomKey Data
    void OnFire(InputValue inputValue) => OnPress(EKeyState.Fire, inputValue);
    void OnPrev(InputValue inputValue) => OnPress(EKeyState.Prev, inputValue);
    void OnNext(InputValue inputValue) => OnPress(EKeyState.Next, inputValue);

    #endregion

    #region Basic Key

    #region Private Variable

    private PlayerInput _playerInput = null;

    #endregion

    #region InputData Variable

    readonly Dictionary<EKeyState, InputData<Vector2>> _vectorDataList = new();
    readonly Dictionary<EKeyState, InputData<bool>> _buttonDataList = new();

    bool _isUIMode = false;

    UpdateDelegateFunction OnUpdateFunction;

    #endregion

    #region Data Referencing

    void RegisterDataRef()
    {
        MoveDirection.Set(ref _vectorDataList[EKeyState.Move].GetDataRef());
        LookDirection.Set(ref _vectorDataList[EKeyState.Look].GetDataRef());
        MousePosition.Set(ref _vectorDataList[EKeyState.MousePoint].GetDataRef());
        MouseScrollWheel.Set(ref _vectorDataList[EKeyState.MouseWheel].GetDataRef());

        EnterButton.Set(ref GetKeyRefData(EKeyState.Enter));
        CancleButton.Set(ref GetKeyRefData(EKeyState.Cancle));

        MouseLeftButton.Set(ref GetKeyRefData(EKeyState.MouseClick));
        MouseMiddleButton.Set(ref GetKeyRefData(EKeyState.MouseMiddleClick));
        MouseRightButton.Set(ref GetKeyRefData(EKeyState.MouseRightClick));
    }

    #endregion

    #region Data Function
    Key<Vector2> GetAxisButtonA(EKeyState keyState)
    {
        if (!_vectorDataList.ContainsKey(keyState)) return null;

        return _vectorDataList[keyState].GetDataRef();
    }
    Key<bool> GetButtonA(EKeyState keyState)
    {
        if (!_buttonDataList.ContainsKey(keyState)) return null;

        return _buttonDataList[keyState].GetDataRef();
    }

    void InitBasicKey()
    {
        if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
        if (_playerInput != null) _isUIMode = _playerInput.currentActionMap.name == "UI";

        RegisterData();
        RegisterDataRef();
    }

    private ref Key<bool> GetKeyRefData(EKeyState keyState)
    {
        return ref _buttonDataList[keyState].GetDataRef();
    }

    private void RegisterData()
    {
        RegisterVectorKeyData(EKeyState.Move);
        RegisterVectorKeyData(EKeyState.Look);
        RegisterVectorKeyData(EKeyState.MousePoint);
        RegisterVectorKeyData(EKeyState.MouseWheel);

        for (EKeyState i = EKeyState.Enter; i < EKeyState.Last; ++i)
            RegisterButtonKeyData(i);

        OnRegisterKey();
    }

    private void RegisterVectorKeyData(EKeyState key)
    {
        if (_vectorDataList.ContainsKey(key)) return;

        _vectorDataList.Add(key, new InputData<Vector2>());
    }

    private void RegisterButtonKeyData(EKeyState key)
    {
        if (_buttonDataList.ContainsKey(key)) return;

        _buttonDataList.Add(key, new InputData<bool>());
    }

    void OnPress(EKeyState key, Vector2 data)
    {
        if (_vectorDataList.ContainsKey(key))
        {
            var curButton = _vectorDataList[key];
            if (curButton == null) return;

            curButton.OnPress(data);
        }
    }

    void OnPress(EKeyState key, InputValue inputValue)
    {
        OnPress(key, inputValue.Get<float>() != 0);
    }

    void OnPress(EKeyState key, bool data)
    {
        if (_buttonDataList.ContainsKey(key))
        {
            var curButton = _buttonDataList[key];
            if (curButton == null) return;

            if (curButton.Data != data)
            {
                if (data)
                {
                    curButton.OnDown(data);

                    OnUpdateFunction += curButton.Press;
                }
                else
                {
                    curButton.OnUp(data);

                    OnUpdateFunction -= curButton.Press;
                }
            }
        }
    }
    #endregion

    #region Input Receive Function

    void OnMove(InputValue inputValue)
    {
        OnPress(EKeyState.Move, inputValue.Get<Vector2>());
    }
    void OnLook(InputValue inputValue)
    {
        OnPress(EKeyState.Look, inputValue.Get<Vector2>());
    }

    void OnSubmit(InputValue inputValue)
    {
        OnPress(EKeyState.Enter, inputValue);
    }
    void OnCancle(InputValue inputValue)
    {
        OnPress(EKeyState.Cancle, inputValue);
    }

    void OnPoint(InputValue inputValue)
    {
        OnPress(EKeyState.MousePoint, inputValue.Get<Vector2>());
    }

    void OnClick(InputValue inputValue)
    {
        OnPress(EKeyState.MouseClick, inputValue);
    }
    void OnMiddleClick(InputValue inputValue)
    {
        OnPress(EKeyState.MouseMiddleClick, inputValue);
    }
    void OnRightClick(InputValue inputValue)
    {
        OnPress(EKeyState.MouseRightClick, inputValue);
    }
    void OnScrollWheel(InputValue inputValue)
    {
        OnPress(EKeyState.MouseWheel, inputValue.Get<Vector2>());
    }

    #endregion

    #endregion

    #region Unity Function
    private void Awake()
    {
        InitSingleton();

        InitBasicKey();
    }

    private void Update() => OnUpdateFunction?.Invoke();
    #endregion

    #region Singleton

    static InputSystem _instance;
    public static InputSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                if (GameObject.FindObjectOfType<PlayerInput>().gameObject)
                {
                    var retVal = GameObject.FindObjectOfType<PlayerInput>().gameObject.GetComponent<InputSystem>();
                    if (!retVal)
                        retVal = GameObject.FindObjectOfType<PlayerInput>().gameObject.AddComponent<InputSystem>();

                    _instance = retVal;
                }
            }

            return _instance;
        }
    }

    void InitSingleton()
    {
        if (_instance != null)
        {
            if (GetComponent<PlayerInput>())
                Destroy(this);
            else
                Destroy(gameObject);
        }
        else
            _instance = this;
    }

    #endregion

    #region Define

    public delegate void InputButton<T>(T data);

    delegate void UpdateDelegateFunction();

    [System.Serializable]
    public class Key<T> : DataPointer<T>
    {
        public InputButton<T> OnPress;
        public InputButton<T> OnUp;
        public InputButton<T> OnDown;
    }

    [System.Serializable]
    public class InputKey<T> : DataReferencer<T>
    {
        public void Set(ref Key<T> data) => _dataPtr = data;
        public Key<T> Get() => (Key<T>)_dataPtr;

        public static implicit operator Key<T>(InputKey<T> d)
        {
            return (Key<T>)d._dataPtr;
        }
    }

    public class InputData<T>
    {
        public void Press()
        {
            OnPress(Data);
        }

        public void OnPress(T data)
        {
            UpdateData(data);

            _dataPtr.OnPress?.Invoke(data);
        }

        public void OnUp(T data)
        {
            UpdateData(data);

            _dataPtr.OnUp?.Invoke(data);
        }
        public void OnDown(T data)
        {
            UpdateData(data);

            _dataPtr.OnDown?.Invoke(data);
        }

        public T Data { get { return _dataPtr.Data; } }

        private Key<T> _dataPtr = new();

        void UpdateData(T data)
        {
            _dataPtr.Data = data;
        }
        public ref Key<T> GetDataRef() { return ref _dataPtr; }
    }
    #endregion
}

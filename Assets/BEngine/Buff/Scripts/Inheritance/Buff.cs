using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff
{
    public void Active() => ++Count;

    protected virtual void Start() => StartBase();
    public virtual void Update(float deltaTime) => UpdateBase(deltaTime);
    public virtual void End() => EndBase();
    public virtual void Run() { }

    public DelegateFunction UpdateDelegate;
    public DelegateFunction EndDelegate;

    public string Name { get => _name; set => _name = value; }
    public int Count
    {
        get => _count;
        set
        {
            _count = value;

            if (value > 0)
                Enable = true;
            else
                End();
        }
    }
    public float Interval { get => _interval; set => _interval = value; }
    public bool Enable { get => _active; set => _active = value; }
    public GameObject Character { get => _character; set => _character = value; }

    private void StartBase()
    {
    }

    private void UpdateBase(float deltaTime)
    {
        if (!_active) return;

        _currentTime += deltaTime;
        if (_currentTime > Interval)
        {
            _currentTime = 0.0f;
            --Count;

            if (UpdateDelegate != null)
                UpdateDelegate?.Invoke();

        }
    }

    private void EndBase()
    {
        if (EndDelegate != null)
            EndDelegate?.Invoke();

        Enable = false;
    }

    GameObject _character = null;

    string _name = "";
    int _count = 0;
    float _interval = 0.0f;
    bool _active = false;

    public Buff(GameObject character, string name, float interval)
    {
        _character = character;
        _name = name;
        _interval = interval;

        UpdateDelegate += Run;

        Start();
    }

    float _currentTime = 0.0f;
}

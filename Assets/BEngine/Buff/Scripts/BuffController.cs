using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public Buff TakeBuff(Buff buff) => TakeBuffA(buff);
    public Buff TakeBuff(Buff buff, int tick) => TakeBuffA(buff, tick);
    public Buff TakeBuff(string name, float interval) => TakeBuffA(name, interval);
    public Buff TakeBuff(string name, float interval, int tick) => TakeBuffA(name, interval, tick);

    public bool IsBuffEnable(Buff buff) => _list.ContainsValue(buff) && buff.Enable;
    public bool IsBuffEnable(string name) => _list.ContainsKey(name);
    
    #region Variable


    readonly Dictionary<string, Buff> _list = new();
    readonly List<Buff> _array = new();

    #endregion

    #region Unity Function
    private void FixedUpdate()
    {
        foreach (var i in _array)
        {
            if (i == null) continue;

            i?.Update(Time.fixedDeltaTime);
        }
    }
    #endregion

    #region Private Function
    private Buff TakeBuffA(Buff buff, int tick)
    {
        if (buff == null) return null;

        for (int i = 0; i < tick; ++i)
            TakeBuff(buff);

        return buff;
    }

    private Buff TakeBuffA(Buff buff)
    {
        if (buff == null) return null;

        bool IsContain = _list.ContainsKey(buff.Name);

        Buff currentBuff = IsContain ? _list[buff.Name] : buff;
        if (!IsContain)
        {
            _list.Add(buff.Name, currentBuff);
            _array.Add(currentBuff);
        }

        currentBuff.Active();
        return buff;
    }

    private Buff TakeBuffA(string name, float interval)
    {
        bool IsContain = _list.ContainsKey(name);

        Buff currentBuff = IsContain ? _list[name] : new(gameObject, name, interval);
        if (!IsContain)
        {
            _list.Add(name, currentBuff);
            _array.Add(currentBuff);
        }

        currentBuff.Active();

        return currentBuff;
    }
    private Buff TakeBuffA(string name, float interval, int tick)
    {
        return TakeBuffA(new Buff(gameObject, name, interval), tick);
    } 
    #endregion
}

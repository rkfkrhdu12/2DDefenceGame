using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private BuffController _buffController = null;

    private void Awake()
    {
        _buffController = GetComponent<BuffController>();
        if (_buffController == null)
        {
            _buffController = gameObject.AddComponent<BuffController>();
        }


    }
}

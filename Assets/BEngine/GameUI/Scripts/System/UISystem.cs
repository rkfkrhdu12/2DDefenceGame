using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{

    #region Singleton
    static UISystem _instance;
    public static UISystem Instance
    {
        get
        {
            if (_instance == null)
            {
                if (GameObject.FindObjectOfType<Canvas>().gameObject)
                {
                    var retVal = GameObject.FindObjectOfType<Canvas>().gameObject.GetComponent<UISystem>();
                    if (!retVal)
                        retVal = GameObject.FindObjectOfType<Canvas>().gameObject.AddComponent<UISystem>();

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
            if (GetComponent<Canvas>())
                Destroy(this);
            else
                Destroy(gameObject);
        }
        else
            _instance = this;
    }

    #endregion

    #region Unity Function

    private void Awake()
    {
        InitSingleton();
    }

    #endregion
}

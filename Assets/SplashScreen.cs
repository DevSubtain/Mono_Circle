using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public static SplashScreen instance;
    public static SplashScreen Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<SplashScreen>();
            return instance;
        }
    }

    public GameObject splash_Obj;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        splash_Obj = transform.parent.gameObject;
    }
    public void Close_SplashScreen()
    {
        splash_Obj.gameObject.SetActive(false);
    }
}

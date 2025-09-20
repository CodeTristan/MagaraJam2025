using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager instance;

    public void Init()
    {
        Debug.Log("MainSceneManager Init");
        instance = this;

        InputManager.instance.DeleteMainSceneControls();
        InputManager.instance.InitMainSceneControls();
        InputManager.instance.EnableMainSceneControl();
    }
}

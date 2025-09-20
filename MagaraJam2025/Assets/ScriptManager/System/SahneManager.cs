using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SahneManager : MonoBehaviour
{
    public static SahneManager instance;

    public Scene currentScene;
    public string currentSceneName;
    public void Init()
    {
        instance = this;
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string name,bool isLoadingData = false)
    {
        if(currentSceneName == "MainScene")
        {
            if(isLoadingData)
            {
                FindFirstObjectByType<MainSceneManager>().Init();
                return; //DO NOT CHANGE THE SCENE
            }
            else
            {
                InputManager.instance.DeleteMainSceneControls();
            }
        }
        currentScene = SceneManager.GetSceneByName(name);
        currentSceneName = name;
        SceneManager.LoadScene(name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BackgroundManager.instance.FindCamera();
        CanvasManager.instance.GetCanvases();
        CanvasManager.instance.AdjustCanvasScalers();
        CanvasManager.instance.AdjustCameraScales();

        if (scene.name == "MainScene")
        {
            FindFirstObjectByType<MainSceneManager>().Init();

            BackgroundManager.instance.ToggleBackgroundCanvas(true);
            InputManager.instance.EnableLogControls();

            List<Condition> conditions = new List<Condition> { new CheckCondition(ConditionNameGroup.Game,GameConditionName.GameStart, 1) };    
            if (ConditionManager.instance.CheckConditions(conditions, false, false))
            {
                DialogManager.instance.StartBranch(DialogManager.instance.FirstBranch);
                ConditionManager.instance.ChangeCondition(GameConditionName.GameStart, 0);
            }
        }
        else
        {
            BackgroundManager.instance.ToggleBackgroundCanvas(false);
            InputManager.instance.DisableLogControls();
            InputManager.instance.DisableMainSceneControl();
            Debug.Log(scene.name);
        }
    }
}

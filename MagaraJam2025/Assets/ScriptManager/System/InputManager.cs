using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlType
{
    MainScene,
    Choice,
    EventChoice,
    Dialog
}
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerActions playerActions;

    public List<ControlType> ActiveControls;


    bool MainSceneInited = false;
    public void Init()
    {
        instance = this;
        ActiveControls = new List<ControlType>();
        playerActions = new PlayerActions();
    }

    public void EnableControls(ControlType[] controlTypes)
    {
        foreach (ControlType controlType in controlTypes)
        {
            EnableControl(controlType);
        }
    }
    public void EnableControl(ControlType controlType)
    {
        switch (controlType)
        {
            case ControlType.MainScene: EnableMainSceneControl(); break;
            case ControlType.Dialog: EnableDialogControls(); break;
            case ControlType.Choice: EnableChoiceControls(); break;
            case ControlType.EventChoice: EnableEventChoiceControls(); break;
        }
    }
    public void EnableChoiceControls()
    {
        playerActions.ChoiceControls.Enable();
        playerActions.ChoiceControls.ConfirmChoice.performed += ChoiceManager.instance.InputConfirmChoice;
        ActiveControls.Add(ControlType.Choice);
    }
    public void DisableChoiceControls()
    {
        playerActions.ChoiceControls.ConfirmChoice.performed -= ChoiceManager.instance.InputConfirmChoice;
        ActiveControls.Remove(ControlType.Choice);
        playerActions.ChoiceControls.Disable();
    }

    public void EnableEventChoiceControls()
    {
        playerActions.ChoiceControls.Enable();
        ActiveControls.Add(ControlType.EventChoice);
        playerActions.ChoiceControls.ConfirmChoice.performed += ChoiceManager.instance.InputEventConfirmChoice;
    }
    public void DisableEventChoiceControls()
    {
        playerActions.ChoiceControls.ConfirmChoice.performed -= ChoiceManager.instance.InputEventConfirmChoice;
        ActiveControls.Remove(ControlType.EventChoice);
        playerActions.ChoiceControls.Disable();
    }
    public void EnableDialogControls()
    {
        playerActions.DialogControls.Enable();

        ActiveControls.Add(ControlType.Dialog);
    }
    public void DisableDialogControls()
    {
        ActiveControls.Remove(ControlType.Dialog);

        playerActions.DialogControls.Disable();
    }
    public void EnableMainSceneControl()
    {
        playerActions.MainSceneControls.Enable();

        ActiveControls.Add(ControlType.MainScene);
    }

    public void DisableMainSceneControl()
    {

        playerActions.MainSceneControls.Disable();

        ActiveControls.Remove(ControlType.MainScene);
    }

    public void EnableLogControls()
    {
        playerActions.LogControls.Enable();
        playerActions.LogControls.ShowDialogLog.performed += DialogLogManager.instance.OpenDialogLog;
        playerActions.LogControls.CloseDialogLog.performed += DialogLogManager.instance.CloseDialogLog;
    }

    public void DisableLogControls()
    {
        playerActions.LogControls.ShowDialogLog.performed -= DialogLogManager.instance.OpenDialogLog;
        playerActions.LogControls.CloseDialogLog.performed -= DialogLogManager.instance.CloseDialogLog;
        playerActions.LogControls.Disable();
    }
    public void DisableAllControls()
    {
        DisableMainSceneControl();
        DisableDialogControls();
        DisableChoiceControls();
        DisableEventChoiceControls();
        ActiveControls.Clear();
    }

    public void InitMainSceneControls()
    {
        playerActions.MainSceneControls.Enable();

        playerActions.MainSceneControls.Disable();

    }

    public void DeleteMainSceneControls()
    {
        playerActions.MainSceneControls.Enable();


        playerActions.MainSceneControls.Disable();

    }
}

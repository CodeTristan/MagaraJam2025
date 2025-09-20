using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class BackgroundDialog
{
    public BackgroundDialogSO data;
    public List<DialogTriggerState> dialogTriggerStates;
    public BackgroundDialog(BackgroundDialogSO data)
    {
        this.data = data;
        dialogTriggerStates = data.dialogTriggers.Select(trigger => new DialogTriggerState(trigger)).ToList();
    }

    public bool CheckIfAnyDialogToTrigger()
    {
        for (int i = 0; i < data.dialogTriggers.Count; i++)
        {
            var trigger = data.dialogTriggers[i];

            List<Condition> conditions = new List<Condition>();
            conditions.AddRange(trigger.RequiredConditions);
            //Debug.Log($"Checking conditions for dialog trigger: {trigger.fileName+"-"+trigger.branchName}, Repeatable: {trigger.isRepeatable}, Triggered: {dialogTriggerStates[i].isTriggered}");
            if (ConditionManager.instance.CheckConditions(conditions, trigger.isRepeatable, dialogTriggerStates[i].isTriggered))
            {
                return true;
            }
        }

        //There is nothing to trigger
        return false;
    }
    public void CheckDialogToTrigger()
    {
        for (int i = 0; i < data.dialogTriggers.Count; i++)
        {
            var trigger = data.dialogTriggers[i];

            List<Condition> conditions = new List<Condition>();
            conditions.AddRange(trigger.RequiredConditions);
            if (ConditionManager.instance.CheckConditions(conditions, trigger.isRepeatable, dialogTriggerStates[i].isTriggered))
            {
                trigger.TriggerDialog(dialogTriggerStates[i]);
                break;
            }
        }
    }
}

[System.Serializable]
public class BackgroundDialogSaveData
{
    public BackgroundName backgroundName;
    public BackgroundCharacterName characterName;
    public List<DialogTriggerState> dialogTriggerStates;
    public BackgroundDialogSaveData(BackgroundDialog dialog)
    {
        backgroundName = dialog.data.backgroundName;
        characterName = dialog.data.characterName;
        dialogTriggerStates = dialog.dialogTriggerStates;
    }
}

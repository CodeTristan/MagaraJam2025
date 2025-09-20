using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DialogTriggerState
{
    [HideInInspector] public DialogTrigger triggerData;
    [HideInInspector] public Dialog randomDialog = null;
    [HideInInspector] public bool isTriggered;
    [HideInInspector] public bool RandomDialogSelected = false;

    public DialogTriggerState()
    {
        isTriggered = false;
        RandomDialogSelected = false;
    }

    public DialogTriggerState(Dialog randomDialog, bool isTriggered, bool RandomDialogSelected)
    {
        this.randomDialog = randomDialog;
        this.isTriggered = isTriggered;
        this.RandomDialogSelected = RandomDialogSelected;
    }

    public DialogTriggerState(DialogTrigger data)
    {
        triggerData = data;
        isTriggered = false;
        RandomDialogSelected = false;
        randomDialog = null;
    }

    public DialogTriggerState(DialogTriggerState data)
    {
        triggerData = data.triggerData;
        isTriggered = data.isTriggered;
        RandomDialogSelected = data.RandomDialogSelected;
        randomDialog = data.randomDialog;
    }
}

[System.Serializable]
public class DialogTrigger
{
    [SerializeField] private string InspectorName;
    public string branchName;
    public string fileName;
    public bool isRepeatable;
    public bool isRandom;
    [SerializeReference]public List<CheckCondition> RequiredConditions;
    public List<CheckCondition> AffectedConditions;

    public DialogTrigger(string branchName, string fileName, bool isRepeatable, List<CheckCondition> requiredConditions, List<CheckCondition> affectedConditions, bool isRandom = false)
    {
        this.branchName = branchName;
        this.fileName = fileName;
        this.isRepeatable = isRepeatable;
        RequiredConditions = requiredConditions;
        AffectedConditions = affectedConditions;
        this.isRandom = isRandom;
    }

    public void TriggerDialog(DialogTriggerState dialogTriggerState)
    {
        dialogTriggerState.isTriggered = true;
        if (isRandom && dialogTriggerState.RandomDialogSelected == false)
        {
            dialogTriggerState.randomDialog = DialogManager.instance.GetRandomDialog(fileName, branchName);
            dialogTriggerState.RandomDialogSelected = true;
            DialogBranch branch = new DialogBranch();
            branch.instructions.Add(dialogTriggerState.randomDialog);
            DialogManager.instance.StartBranch(branch);
        }
        else if (isRandom && dialogTriggerState.RandomDialogSelected == true)
        {
            DialogBranch branch = new DialogBranch();
            branch.instructions.Add(dialogTriggerState.randomDialog);
            DialogManager.instance.StartBranch(branch);
        }
        else
        {
            DialogManager.instance.StartBranch(fileName, branchName);
        }

        //After dialog editing new conditions
        for (int i = 0; i < AffectedConditions.Count; i++)
        {
            CheckCondition con = AffectedConditions[i];
            ConditionManager.instance.GetCondition(con.ToEnum()).status = con.status;
        }
    }

}

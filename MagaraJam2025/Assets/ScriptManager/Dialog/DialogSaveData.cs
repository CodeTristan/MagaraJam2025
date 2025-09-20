using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogSaveData
{
    public string FileName;
    public string BranchName;
    public ChoiceBodySaveData[] choiceBodySaveData;

    public DialogSaveData()
    {
        FileName = string.Empty;
        BranchName = string.Empty;
        choiceBodySaveData = new ChoiceBodySaveData[0];
    }
    public DialogSaveData(string fileName, string branchName, ChoiceBodySaveData[] choiceSaveData)
    {
        FileName = fileName;
        BranchName = branchName;
        this.choiceBodySaveData = choiceSaveData;
    }
}

[System.Serializable]
public class ChoiceBodySaveData
{
    public int choiceBodyID;
    public ChoiceSaveData[] choiceSaveData;

    public ChoiceBodySaveData()
    {
        choiceBodyID = -1;
        choiceSaveData = new ChoiceSaveData[0];
    }

    public ChoiceBodySaveData(ChoiceBody choiceBody)
    {
        choiceBodyID = choiceBody.id;
        choiceSaveData = new ChoiceSaveData[choiceBody.choices.Count];

        for (int i = 0; i < choiceBody.choices.Count; i++)
        {
            choiceSaveData[i] = new ChoiceSaveData(i, choiceBody.choices[i].hasSelected);
        }
    }
}

[System.Serializable]
public class ChoiceSaveData
{
    public int ChoiceID;
    public bool hasSelected;

    public ChoiceSaveData()
    {
        ChoiceID = -1;
        hasSelected = false;
    }

    public ChoiceSaveData(int choiceID, bool hasSelected)
    {
        ChoiceID = choiceID;
        this.hasSelected = hasSelected;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableBranch : MonoBehaviour
{
    public DialogBranch branch;

    public string fileName;
    public string branchName;
    public string ExamineText = "Examine";


    public void _StartBranchWithFile()
    {
        DialogManager.instance.StartBranch(fileName, branchName);
    }
    public void _StartDialog()
    {
        DialogManager.instance.StartBranch(branch);
    }
}

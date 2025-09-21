using System.Collections.Generic;
using UnityEngine;

public class UIInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string description;
    public string FileName;
    public string BranchName;

    public List<Dialog> dialogs;
    public string GetDescription()
    {
        return description;
    }

    public void OnInteract()
    {
        Debug.Log($"Interacted with UI element: {gameObject.name}");
        if (FileName != "" && BranchName != "")
            DialogManager.instance.StartBranch(FileName, BranchName);
        else
        {
            DialogBranch branch = new DialogBranch();
            branch.instructions.AddRange(dialogs);
            DialogManager.instance.StartBranch(branch);
        }
    }
}

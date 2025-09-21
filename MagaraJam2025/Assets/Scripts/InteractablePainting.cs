using System.Collections.Generic;
using UnityEngine;

public class InteractablePainting : MonoBehaviour, IInteractable
{
    [SerializeField] private string description;
    public GameObject PaintUIObject;
    [SerializeField] private Vector3 Offset;

    public string FileName;
    public string BranchName;

    public List<Dialog> dialogs;
    public string GetDescription()
    {
        return description;
    }

    public void OnInteract()
    {
        Debug.Log($"Interacted with painting: {gameObject.name}");
        BackgroundManager.instance.ToggleBackgroundCanvas(true);
        PaintUIObject.SetActive(true);

        if (FileName != "" && BranchName != "")
            DialogManager.instance.StartBranch(FileName, BranchName);
        else
        {
            DialogBranch branch = new DialogBranch();
            branch.instructions.AddRange(dialogs);
            DialogManager.instance.StartBranch(branch);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

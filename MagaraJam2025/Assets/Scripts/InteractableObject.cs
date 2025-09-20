using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string description;
    public string FileName;
    public string BranchName;
    public string GetDescription()
    {
        return description;
    }

    public void OnInteract()
    {
        Debug.Log($"Interacted with {gameObject.name}");
        DialogManager.instance.StartBranch(FileName, BranchName);
    }
}

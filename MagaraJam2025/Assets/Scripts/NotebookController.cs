using System.Collections.Generic;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    public static NotebookController Instance { get; private set; }

    public bool IsOpen { get; private set; } = false;

    public GameObject NotebookUI;

    public List<string> Clues = new List<string>();

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        NotebookUI.SetActive(false);
    }


}

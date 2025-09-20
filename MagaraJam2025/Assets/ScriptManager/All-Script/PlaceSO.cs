using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewPlace", menuName = "ScriptableObjects/Place")]
public class PlaceSO : ScriptableObject
{
    public PlaceName placeName;
    public BackgroundName backgroundName;
    public bool HasBeenVisited;
    public string MusicName;
    public bool isLocked = true;
    public List<DialogTrigger> dialogTriggers;
}
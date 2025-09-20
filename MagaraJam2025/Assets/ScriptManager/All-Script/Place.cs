using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Place
{
    public PlaceSO PlaceData;
    public bool HasBeenVisited;
    public bool isLocked;
    public List<DialogTriggerState> dialogTriggerStates = new List<DialogTriggerState>();

    public Place(PlaceSO placeSO)
    {
        PlaceData = placeSO;
        HasBeenVisited = placeSO.HasBeenVisited;
        isLocked = placeSO.isLocked;
        dialogTriggerStates = placeSO.dialogTriggers.Select(trigger => new DialogTriggerState(trigger)).ToList();
    }
    public void CheckDialogToTrigger()
    {
        if (PlaceData.dialogTriggers == null || PlaceData.dialogTriggers.Count == 0)
        {
            Debug.LogWarning("No dialog triggers found for place: " + PlaceData.placeName);
            return;
        }
        
        for (int i = 0; i < PlaceData.dialogTriggers.Count; i++)
        {
            DialogTrigger trigger = PlaceData.dialogTriggers[i];
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
public class PlaceSaveData
{
    public PlaceName placeName; 
    public bool HasBeenVisited;
    public bool isLocked;
    public List<DialogTriggerState> dialogTriggerStates;
    public PlaceSaveData(Place place)
    {
        placeName = place.PlaceData.placeName;
        HasBeenVisited = place.HasBeenVisited;
        isLocked = place.isLocked;
        dialogTriggerStates = new List<DialogTriggerState>(place.dialogTriggerStates);
    }
}

[System.Serializable]
public enum PlaceName
{
    Null,
}

[System.Serializable]
public class PlaceUI
{
    [SerializeField] private string InspectorName;
    public PlaceName placeName;
    public Button placeButton;
    public TextMeshProUGUI placeText;
    public GameObject NewText;
}

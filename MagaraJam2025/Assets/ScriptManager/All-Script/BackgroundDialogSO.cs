using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBackgroundDialog", menuName = "ScriptableObjects/BackgroundDialog")]
public class BackgroundDialogSO : ScriptableObject
{
    public BackgroundName backgroundName;
    public BackgroundCharacterName characterName;
    public List<DialogTrigger> dialogTriggers;
    public List<CheckCondition> conditions;
}
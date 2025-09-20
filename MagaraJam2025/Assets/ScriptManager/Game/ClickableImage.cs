using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ClickableImage : MonoBehaviour
{
    public List<CheckCondition> ActivationConditions;
    public Image image;
    public Button button;
    [HideInInspector] public bool isActive;
    public void Init()
    {
        List<Condition> conditions = new List<Condition>();
        conditions.AddRange(ActivationConditions);
        isActive = ConditionManager.instance.CheckConditions(conditions);

        if(button != null) button.interactable = isActive;
        if(image != null) image.enabled = isActive;

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager instance;
    [Header("Conditions")]
    public Dictionary<string, Condition> conditionDictionary;

    public void Init()
    {
        instance = this;
        conditionDictionary = new Dictionary<string, Condition>();

        foreach (var con in GameManager.instance.allConditions.conditions)
            conditionDictionary[con.conditionName] = con;
        foreach (var con in DialogManager.instance.File.Conditions)
            conditionDictionary[con.conditionName] = con;

    }

    public void SetSavedConditions(List<Condition> conditions)
    {
        foreach (var condition in conditions)
        {
            if (conditionDictionary.TryGetValue(condition.conditionName, out var con))
            {
                con.status = condition.status;
            }
            else
            {
                Debug.LogError("Condition not found in ConditionManager when setting saved conditions --> " + condition.conditionName);
            }
        }
    }

    public bool CheckConditions(List<Condition> conditions, bool isRepeatable = false, bool isTriggered = false)
    {
        //It is not repeatable dialog and it is triggered once
        if (isRepeatable == false && isTriggered == true)
            return false;

        if(conditions == null)
            return true;
        if(conditions.Count == 0) 
            return true;
        foreach (Condition c in conditions)
        {

            Condition con = null;
            if (c.GetType() == typeof(CheckCondition))
            {
                CheckCondition cc = (CheckCondition)c;
                con = GetCondition(cc.ToEnum());
            }
            else if (c.GetType() == typeof(Condition))
            {
                con = GetCondition(c.conditionName);
            }
            


            if (con == null)
            {
                Debug.LogError("Returned False in CheckDialogConditions in ConditionManager because condition is null! --> " + c.conditionName);
                return false;
            }
            

            switch (c.checkSign)
            {
                case Condition.CheckSign.Equal:
                    {
                        if (c.status != con.status)
                        {
                            return false;
                        }
                        break;
                    }
                case Condition.CheckSign.NotEqual:
                    {
                        if (c.status == con.status)
                        {
                            return false;
                        }
                        break;
                    }
                case Condition.CheckSign.LessThan:
                    {
                        if (con.status >= c.status)
                        {
                            return false;
                        }
                        break;
                    }
                case Condition.CheckSign.LessThanOrEqual:
                    {
                        if (con.status > c.status)
                        {
                            return false;
                        }
                        break;
                    }
                case Condition.CheckSign.GreaterThan:
                    {
                        if (con.status <= c.status)
                        {
                            return false;
                        }
                        break;
                    }
                case Condition.CheckSign.GreaterThanOrEqual:
                    {
                        if (con.status < c.status)
                        {
                            return false;
                        }
                        break;
                    }
                default:
                    Debug.LogError("Sing error in CheckConditions --> " + con.checkSign);
                    break;
            }
        }

        return true;
    }

    public void AdjustCondition(ConditionInstruction conditionInstruction)
    {
        Condition condition = GetCondition(conditionInstruction.condition.conditionName);
        if(condition == null)
        {
            Debug.LogError("Condition not found! --> " + conditionInstruction.condition.conditionName);
            return;
        }

        int value = 0;
        string text = "Variable adjusted: " + condition.conditionName + " , Operation: " + conditionInstruction.operation +  " , Old Value : " + condition.status + " New Value : ";
        switch (conditionInstruction.operand)
        {
            case ConditionInstruction.ConditionOperand.Constant:
                {
                    value = conditionInstruction.condition.status;
                    break;
                }
            case ConditionInstruction.ConditionOperand.Variable:
                {
                    value = GetCondition(conditionInstruction.operand_VariableName).status;
                    break;
                }
            case ConditionInstruction.ConditionOperand.Random:
                {
                    value = UnityEngine.Random.Range(conditionInstruction.randomValueStart, conditionInstruction.randomValueEnd + 1);
                    break;
                }

        }

        string change = "";

        switch (conditionInstruction.operation)
        {
            case ConditionInstruction.ConditionOperation.Set:
                {
                    condition.status = value;
                    break;
                }
            case ConditionInstruction.ConditionOperation.Add:
                {
                    condition.status += value;
                    change = "+";
                    break;
                }
            case ConditionInstruction.ConditionOperation.Sub:
                {
                    condition.status -= value;
                    change = "-";
                    break;
                }
            case ConditionInstruction.ConditionOperation.Mul:
                {
                    condition.status *= value;
                    break;
                }
            case ConditionInstruction.ConditionOperation.Div:
                {
                    condition.status /= value;
                    break;
                }
            case ConditionInstruction.ConditionOperation.Mod:
                {
                    condition.status = condition.status % value;
                    break;
                }
        }


        Debug.Log(change + "" + value + " " + condition.conditionName);

        text += condition.status;
    }

    public void ChangeCondition(Enum conditionName,int status)
    {
        Condition condition = GetCondition(conditionName);
        if(condition != null)
        {
            condition.status = status;
        }
    }

    public void ChangeCondition(string conditionName, int status)
    {
        Condition condition = GetCondition(conditionName);
        if (condition != null)
        {
            condition.status = status;
        }
    }

    public void IncreaseCondition(Enum conditionName, int status)
    {
        Condition condition = GetCondition(conditionName);
        if (condition != null)
        {
            condition.status += status;
        }
    }
    public Condition GetCondition(Enum name,bool showError = true)
    {
        string ConName = GameManager.instance.allConditions.GetConditionName(name);
        if(ConName == "NULL")
        {
            Debug.LogError("Condition couldn't found in ConditionManager --> " + name);
            return null;
        }

        return GetCondition(ConName, showError);
    }

    public Condition GetCondition(string name, bool showError = true)
    {
        if (conditionDictionary.TryGetValue(name, out var condition))
            return condition;

        if (showError)
            Debug.LogError("Condition not found in ConditionManager --> " + name);
        return null;
    }

}

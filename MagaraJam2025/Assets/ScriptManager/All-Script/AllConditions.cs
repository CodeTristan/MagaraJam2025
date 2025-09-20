using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using static Condition;

[System.Serializable]
public class Condition
{
    [XmlElement("ConditionName")] public string conditionName;

    [XmlElement("Status")] public int status;

    [XmlElement("Type")] public ConditionType type;

    [XmlElement("CheckSign")] public CheckSign checkSign;

    public Condition()
    {
        checkSign = CheckSign.Equal;
    }
    public Condition(string conditionName, int status, ConditionType type = ConditionType.Switch, CheckSign checkSign = CheckSign.Equal)
    {
        this.conditionName = conditionName;
        this.type = type;
        this.status = status;
        this.checkSign = checkSign;
    }

    public Condition(Condition original)
    {
        conditionName = original.conditionName;
        status = original.status;
        type = original.type;
        checkSign = original.checkSign;
    }

    public Condition(XmlNode node)
    {
        conditionName = node["ConditionName"].InnerText;
        status = int.Parse(node["Status"].InnerText);
        type = (ConditionType)Enum.Parse(typeof(ConditionType), node["Type"].InnerText);

        if (Enum.TryParse(typeof(CheckSign), node["CheckSign"].InnerText, out var result))
            checkSign = (CheckSign)result;
        else
        {
            Debug.LogError("CheckSign not found in XML: " + node["CheckSign"].InnerText);
            checkSign = EnumExtensions.ConvertSign(node["CheckSign"].InnerText);
        }

    }

    public override string ToString()
    {
        return "Name: " + conditionName + " Status: " + status + " Sign: " + checkSign;
    }

    public enum ConditionType
    {
        [XmlEnum("Switch")] Switch,
        [XmlEnum("Variable")] Variable
    }

    public enum CheckSign
    {
        [DisplayName("=")] Equal,
        [DisplayName("!=")] NotEqual,
        [DisplayName(">")] GreaterThan,
        [DisplayName("<")] LessThan,
        [DisplayName(">=")] GreaterThanOrEqual,
        [DisplayName("<=")] LessThanOrEqual
    }

}

[System.Serializable]
public class CheckCondition : Condition
{
    [SerializeField] private ConditionNameGroup group;
    [SerializeField] private int id;

    public CheckCondition()
    {
        checkSign = CheckSign.Equal;
    }
    public CheckCondition(ConditionNameGroup group, Enum id, int status, ConditionType type = ConditionType.Switch, CheckSign checkSign = CheckSign.Equal)
    {
        this.group = group;
        this.id = Convert.ToInt32(id);
        this.type = type;
        this.status = status;
        this.checkSign = checkSign;
    }

    public override string ToString()
    {
        return "Name: " + ToEnum() + " Status: " + status + " Sign: " + checkSign;
    }

    public Enum ToEnum()
    {
        return group switch
        {
            ConditionNameGroup.Game => (GameConditionName)id,
            _ => throw new ArgumentException("Invalid condition name group: " + id)
        };
    }
}


[System.Serializable]

public class AllConditions
{
    public List<Condition> conditions;

    public AllConditions()
    {
        conditions = new List<Condition>();

        Type[] enumTypes =
{
            typeof(GameConditionName)
        };

        foreach (var enumType in enumTypes)
        {
            foreach (Enum e in Enum.GetValues(enumType))
            {
                string conditionName = e.GetDisplay();
                int defaultValue = e.GetDefault();
                Condition condition = new Condition(conditionName,defaultValue);
                conditions.Add(condition);
            }
        }
    }


    public string GetConditionName(Enum conditionEnum)
    {
        // Extension metodunu doðrudan kullanýyoruz
        string name = conditionEnum.GetDisplay();


        return string.IsNullOrEmpty(name) ? "NULL" : name;
    }


}


public static class EnumExtensions
{
    public static CheckSign ConvertSign(string value)
    {
        return value switch
        {
            "=" => CheckSign.Equal,
            "!=" => CheckSign.NotEqual,
            "<" => CheckSign.LessThan,
            "<=" => CheckSign.LessThanOrEqual,
            ">" => CheckSign.GreaterThan,
            ">=" => CheckSign.GreaterThanOrEqual,
            _ => throw new ArgumentException("Invalid check sign: " + value)
        };
    }

    public static string GetDisplay(this Enum e)
    {
        FieldInfo fi = e.GetType().GetField(e.ToString());
        if (fi == null) return e.ToString();

        var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(
                        fi, typeof(DisplayNameAttribute));

        return attr != null ? attr.display : e.ToString();
    }

    public static int GetDefault(this Enum e)
    {
        FieldInfo fi = e.GetType().GetField(e.ToString());
        if (fi == null) return 0;

        var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(
                        fi, typeof(DisplayNameAttribute));

        return attr != null ? attr.defaultValue : 0;
    }
}
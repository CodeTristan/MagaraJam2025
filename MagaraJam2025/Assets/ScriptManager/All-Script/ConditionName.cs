using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionNameGroup
{
    Game = 0,
}

public enum GameConditionName
{
    [DisplayName("GameStart", 1)] GameStart = 0,
}


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DisplayNameAttribute : Attribute
{
    public readonly string display;
    public readonly int defaultValue;   // Ýstersen baþka meta da ekle

    public DisplayNameAttribute(string display, int defaultValue = 0)
    {
        this.display = display;
        this.defaultValue = defaultValue;
    }
}
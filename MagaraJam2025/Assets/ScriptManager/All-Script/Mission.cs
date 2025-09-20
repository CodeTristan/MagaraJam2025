using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class Mission
{
    [XmlElement("Name")] public string name;
    [XmlElement("DisplayName")] public string displayName;
    [XmlElement("Type")] public MissionType type;

    [XmlIgnore] public bool isCompleted;
    [XmlIgnore] public bool isActive;

    [XmlIgnore] public int objectiveIndex;
    [XmlIgnore] public List<Objective> activeObjectives;
    [XmlElement("Objective")] public List<Objective> objectives;

    public Mission()
    {
        objectives = new List<Objective>();
        activeObjectives = new List<Objective>();
    }
    public Mission(string name, MissionType type, List<Objective> objectives) : this()
    {
        this.name = name;
        this.type = type;
        this.objectives = objectives;
    }

    public Mission(XmlNode node) : this()
    {
        name = node["Name"].InnerText;
        displayName = node["DisplayName"].InnerText;
        type = (MissionType)Enum.Parse(typeof(MissionType), node["Type"].InnerText);

        XmlNodeList objectiveNodes = node.SelectNodes("Objective");
        foreach (XmlNode objNode in objectiveNodes)
        {
            if (objNode["ObjectiveType"] != null && objNode["ObjectiveType"].InnerText == "Collectible")
            {
                CollectibleObjective collectibleObjective = new CollectibleObjective(objNode);
                objectives.Add(collectibleObjective);
                continue;
            }
            if (objNode["ObjectiveType"] != null && objNode["ObjectiveType"].InnerText == "Standart")
            {
                Objective objective = new Objective(objNode);
                objectives.Add(objective);
                continue;
            }
        }
    }

    public void StartMission()
    {
        objectiveIndex = 0;
        isActive = true;
        StartNextObjective();
    }

    public void StartObjective(Objective objective)
    {
        bool isCorrectObjective = false;
        int index = 0;
        foreach (var item in objectives)
        {
            if(objective == item)
            {
                isCorrectObjective = true;
                break;
            }
            index++;
        }
        if(isCorrectObjective == false)
        {
            Debug.LogError("Objective is not in this mission!! Mission: " + name + " --> Objective: " + objective.name);
            return;
        }

        objective.isVisible = true;
        objectiveIndex = index + 1;
        activeObjectives.Add(objective);

    }

    public bool StartNextObjective()
    {
        if(objectiveIndex == objectives.Count)
        {
            Debug.Log("All Objectives have started already!! --> " + name);
            return false;
        }

        StartObjective(objectives[objectiveIndex]);
        return true;
    }
    public void CompleteObjective(Objective objective)
    {
        bool isCorrectObjective = false;
        foreach (var item in objectives)
        {
            if (objective == item)
            {
                isCorrectObjective = true;
                break;
            }
        }
        if (isCorrectObjective == false)
        {
            Debug.LogError("Objective is not in this mission!! Mission: " + name + " --> Objective: " + objective.name);
            return;
        }

        activeObjectives.Remove(objective);
        objective.CompleteObjective();
    }
    public void CompleteMission()
    {
        isCompleted = true;
        isActive = false;

    }
}
[System.Serializable]
public class CollectibleObjective : Objective
{
    [XmlElement("MaxProgress")] public float MaxProgress;
    [XmlElement("CurrentProgress")] public float CurrentProgress;

    public CollectibleObjective() : base()
    {
        objectiveType = ObjectiveType.Collectible;
    }

    public CollectibleObjective(string name, string description, string helpText, bool isVisible, float maxProgress) : base(name, description, helpText, isVisible, ObjectiveType.Collectible)
    {
        MaxProgress = maxProgress;
        CurrentProgress = 0;
    }

    public CollectibleObjective(Objective objective)
    {
        name = objective.name;
        description = objective.description;
        helpText = objective.helpText;
        isVisible = objective.isVisible;
        objectiveType = ObjectiveType.Collectible;
        MaxProgress = 0;
        CurrentProgress = 0;
    }

    public CollectibleObjective(XmlNode node) : base(node)
    {
        if (node["MaxProgress"] != null)
            MaxProgress = float.Parse(node["MaxProgress"].InnerText);
        else
            MaxProgress = 0;

        if (node["CurrentProgress"] != null)
            CurrentProgress = float.Parse(node["CurrentProgress"].InnerText);
        else
            CurrentProgress = 0;
    }

    public void AddProgress(float amount)
    {
        CurrentProgress += amount;
        if (CurrentProgress >= MaxProgress)
        {
            CurrentProgress = MaxProgress;
            Debug.Log(name + " Objective Completed!");
        }
    }
}
[System.Serializable]
[XmlInclude(typeof(CollectibleObjective))]
public class Objective
{
    [XmlElement("Name")] public string name;
    [XmlElement("Description")] public string description;
    [XmlElement("HelpText")] public string helpText;
    [XmlElement("ObjectiveType")] public ObjectiveType objectiveType;

    public bool isCompleted;
    public bool isVisible;

    public Objective()
    {

    }
    public Objective(string name, string description, string helpText, bool isVisible, ObjectiveType objectiveType)
    {
        this.name = name;
        this.description = description;
        this.helpText = helpText;
        this.isVisible = isVisible;
        this.objectiveType = objectiveType;
    }

    public Objective(Objective objective)
    {
        name = objective.name;
        description = objective.description;
        helpText = objective.helpText;
        isVisible = objective.isVisible;
        objectiveType = objective.objectiveType;
        isCompleted = objective.isCompleted;
    }

    public Objective(XmlNode node)
    {
        name = node["Name"].InnerText;
        description = node["Description"].InnerText;
        helpText = node["HelpText"].InnerText;

        if (node["ObjectiveType"] == null)
            objectiveType = ObjectiveType.Standart;
        else
            objectiveType = (ObjectiveType)Enum.Parse(typeof(ObjectiveType), node["ObjectiveType"].InnerText);
    }

    public void CompleteObjective()
    {
        isCompleted = true;
    }
}
[System.Serializable]
public enum MissionType
{
    [XmlEnum("Main")] Main,
    [XmlEnum("Side")] Side
}

[System.Serializable]
public enum ObjectiveType
{
    [XmlEnum("Standart")] Standart,
    [XmlEnum("Collectible")] Collectible
}


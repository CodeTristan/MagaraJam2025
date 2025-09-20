using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
[XmlRoot("File")]
public class ExportableFile
{
    [XmlElement("FileName")]
    private string FileName;

    [XmlElement("IDCounter")]
    public int IDCounter;

    [XmlElement("InnerFiles")]
    public List<FileCluster> FileClusters;

    [XmlElement("Condition")]
    public List<Condition> Conditions;

    [XmlElement("Mission")]
    public List<Mission> Missions;
    public ExportableFile()
    {
        FileName = "Dialogs";
        FileClusters = new List<FileCluster>();
        Conditions = new List<Condition>();
        Missions = new List<Mission>();
    }

    public ExportableFile(XmlNode node) : this()
    {
        XmlNodeList clusters = node.SelectNodes("/File/InnerFiles");
        foreach (XmlNode n in clusters)
        {
            FileCluster cluster = new FileCluster(n);
            FileClusters.Add(cluster);
        }

        XmlNodeList conditionNodes = node.SelectNodes("/File/Condition");
        foreach (XmlNode n in conditionNodes)
        {
            Condition con = new Condition(n);
            Conditions.Add(con);
        }

        XmlNodeList missionNodes = node.SelectNodes("/File/Mission");
        foreach (XmlNode n in missionNodes)
        {
            Mission mission = new Mission(n);
            Missions.Add(mission);
        }
    }

    public ExportableFile Clone()
    {
        string jsonString = JsonUtility.ToJson(this);
        ExportableFile fileCopy = JsonUtility.FromJson<ExportableFile>(jsonString);
        return fileCopy;
    }
}

[System.Serializable]
[XmlRoot("SaveFile")]
public class FileXML
{
    [XmlElement("FileName")]
    public string FileName;

    [XmlElement("ClusterName")]
    public string ClusterName;

    [XmlElement("IDCounter")]
    public int IDCounter;

    [XmlElement("BranchCluster")]
    public List<BranchCluster> BranchClusters;


    public FileXML()
    {
        BranchClusters = new List<BranchCluster>();
        IDCounter = 0;
    }

    public FileXML(XmlNode node) : this()
    {
        FileName = node["FileName"].InnerText;
        ClusterName = node["ClusterName"].InnerText;
        IDCounter = int.Parse(node["IDCounter"].InnerText);
        XmlNodeList instructionNodes = node.SelectNodes("BranchCluster");
        foreach (XmlNode instructionNode in instructionNodes)
        {
            BranchCluster branchCluster = new BranchCluster(instructionNode);
            BranchClusters.Add(branchCluster);
        }
    }

    public FileXML Clone()
    {
        FileXML fileXML = new FileXML();

        fileXML.FileName = FileName;
        fileXML.ClusterName = ClusterName;
        fileXML.IDCounter = IDCounter;

        for (int i = 0; i < BranchClusters.Count; i++)
        {
            fileXML.BranchClusters.Add(BranchClusters[i].Clone());
        }

        return fileXML;
    }
}


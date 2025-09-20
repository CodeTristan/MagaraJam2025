using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Unity.VisualScripting;

[System.Serializable]
public class FileCluster
{

    [XmlElement("Name")] public string Name;
    [XmlElement("File")] public List<FileXML> Files;

    public FileCluster()
    {
        Files = new List<FileXML>();
    }

    public FileCluster(XmlNode root) : this()
    {
        Name = root["Name"].InnerText;

        XmlNodeList nodes = root.SelectNodes("File");
        foreach (XmlNode node in nodes)
        {
            FileXML cluster = new FileXML(node);
            Files.Add(cluster);
        }
    }

    public FileCluster Clone()
    {
        FileCluster clone = new FileCluster();
        clone.Name = Name;

        for (int i = 0; i < Files.Count; i++)
        {
            clone.Files.Add(Files[i].Clone());
        }

        return clone;
    }
}

[System.Serializable]
public class BranchCluster
{

    [XmlElement("Name")] public string Name;
    [XmlElement("Branch")] public List<DialogBranch> Branches;

    public BranchCluster()
    {
        Branches = new List<DialogBranch>();
    }

    public BranchCluster(string Name) : this()
    {
        this.Name = Name;
    }

    public BranchCluster(XmlNode root) : this()
    {
        Name = root["Name"].InnerText;

        XmlNodeList nodes = root.SelectNodes("Branch");
        foreach (XmlNode node in nodes)
        {
            DialogBranch branch = new DialogBranch(node);
            if(branch.IsStartBranch)
                DialogManager.instance.FirstBranch = branch;
            Branches.Add(branch);
        }
    }

    public BranchCluster Clone()
    {
        BranchCluster clone = new BranchCluster();

        clone.Name = Name;

        for (int i = 0; i < Branches.Count; i++)
        {
            clone.Branches.Add(new DialogBranch(Branches[i]));
        }

        return clone;
    }
}

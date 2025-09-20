using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    [SerializeField] private TextAsset DialogXml;
    XmlDocument dialogDataXML;

    public void Init()
    {
        instance = this;
        if(GameManager.instance.DebugMod)
        {
            Debug.Log("XMLManager initialized in Debug Mode");
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Debug"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Debug");
            }
        }

    }

    public List<Condition> GetConditions(TextAsset textAsset)
    {
        XmlDocument conditionXML = new XmlDocument();
        List<Condition> conditions = new List<Condition>();
        conditionXML.LoadXml(textAsset.text);

        XmlNodeList conditionNodes = conditionXML.SelectNodes("/SaveData/Condition");
        foreach (XmlNode node in conditionNodes)
        {
            Condition con = new Condition(node);
            conditions.Add(con);
        }

        return conditions;
    }
    public ExportableFile GetInstructions(TextAsset textAsset)
    {
        dialogDataXML = new XmlDocument();
        dialogDataXML.LoadXml(textAsset.text);

        ExportableFile file = new ExportableFile(dialogDataXML);
        return file;
    }

    public ExportableFile GetInstructions()
    {
        dialogDataXML = new XmlDocument();
        dialogDataXML.LoadXml(DialogXml.text);

        ExportableFile file = new ExportableFile(dialogDataXML);
        return file;
    }


}

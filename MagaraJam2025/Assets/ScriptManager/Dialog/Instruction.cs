    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System;


[System.Serializable]
[XmlInclude(typeof(Dialog)), XmlInclude(typeof(ChoiceBody)), XmlInclude(typeof(DialogBranch)) , 
XmlInclude(typeof(Miscellaneous)), XmlInclude(typeof(Instruction)), XmlInclude(typeof(ConditionInstruction)),
XmlInclude(typeof(BackgroundInstruction)), XmlInclude(typeof(ConditionalBranchInstruction)), XmlInclude(typeof(WaitInstruction)),
XmlInclude(typeof(JumpBranchInstruction)), XmlInclude(typeof(InstructionType)), XmlInclude(typeof(MissionInstruction)),
XmlInclude(typeof(MissionInstruction.MissionInstructionType))]
public class Instruction
{
    [XmlElement("Name")] public string name;
    [XmlElement("Type")] public InstructionType type;
    [XmlElement("ID")] public int id;

    public Instruction() 
    {

    }

    public Instruction(Instruction ins)
    {
        name = ins.name;
        type = ins.type;
    }
    public Instruction(string name, InstructionType type,int id)
    {
        this.name = name;
        this.type = type;
        this.id = id;
    }
    public Instruction(XmlNode node) 
    {
        if (node.SelectSingleNode("Name") == null)
            name = "NONE";
        else
            name = node.SelectSingleNode("Name").InnerText;
        type = (InstructionType)Enum.Parse(typeof(InstructionType),node.SelectSingleNode("Type").InnerText);
        id = int.Parse(node.SelectSingleNode("ID").InnerText);
    }

    public override string ToString()
    {
        return "Name: " + name + " Type: " + type + " ID: " + id;
    }
}

[System.Serializable]
public enum InstructionType
{
    Dialog,
    DialogBranch,
    ChoiceBody,
    Background,
    Music,
    Minigame,
    ConditionSwitchInstruction,
    ConditionVariableInstruction,
    WaitInstruction,
    ConditionalBranchInstruction,
    JumpBranchInstruction,
    MissionInstruction
}
[System.Serializable]
public class DialogBranch : Instruction
{
    [XmlElement("IDCounter")] public int IDCounter;
    [XmlElement("ClusterName")] public string ClusterName;
    [XmlElement("IsStartBranch")] public bool IsStartBranch;

    [XmlElement("Instruction")]
    [SerializeReference] public List<Instruction> instructions;

    [XmlIgnore]
    [SerializeField] public List<Dialog> dialogs;

    [XmlIgnore]
    [SerializeReference] public List<ChoiceBody> choiceBodies;

    [XmlIgnore]
    [SerializeReference] public List<Miscellaneous> miscellaneouses;

    [XmlIgnore]
    [SerializeReference] public List<ConditionInstruction> Conditions;

    [XmlIgnore]
    [SerializeReference] public List<ConditionalBranchInstruction> ConditionalBranches;

    public DialogBranch()
    {
        name = "NONE";
        IsStartBranch = false;
        type = InstructionType.DialogBranch;
        IDCounter = 0;
        dialogs = new List<Dialog>();
        choiceBodies = new List<ChoiceBody>();
        miscellaneouses = new List<Miscellaneous>();
        instructions = new List<Instruction>();
        Conditions = new List<ConditionInstruction>();
        ConditionalBranches = new List<ConditionalBranchInstruction>();
    }
    public DialogBranch(int id) : this()
    {

        this.id = id;

    }

    public DialogBranch(DialogBranch branch) : this(0)
    {
        name = branch.name;
        IDCounter = branch.IDCounter;
        foreach (var ins in branch.instructions)
        {
            Instruction item = new Instruction(ins);
            switch (ins.type)
            {
                case InstructionType.Dialog: item = new Dialog((Dialog)ins); dialogs.Add((Dialog)item); break;
                case InstructionType.ChoiceBody: item = new ChoiceBody((ChoiceBody)ins); choiceBodies.Add((ChoiceBody)ins); break;
                case InstructionType.Background: item = new BackgroundInstruction((BackgroundInstruction)ins); miscellaneouses.Add((BackgroundInstruction)item); break;
                case InstructionType.Music: item = new Miscellaneous((Miscellaneous)ins); miscellaneouses.Add((Miscellaneous)item); break;
                case InstructionType.Minigame: item = new Miscellaneous((Miscellaneous)ins); miscellaneouses.Add((Miscellaneous)item); break;
                case InstructionType.ConditionSwitchInstruction: item = new ConditionInstruction((ConditionInstruction)ins); Conditions.Add((ConditionInstruction)ins); miscellaneouses.Add((ConditionInstruction)item); break;
                case InstructionType.ConditionVariableInstruction: item = new ConditionInstruction((ConditionInstruction)ins); Conditions.Add((ConditionInstruction)ins); ; miscellaneouses.Add((ConditionInstruction)item); break;
                case InstructionType.WaitInstruction: item = new WaitInstruction((WaitInstruction)ins); miscellaneouses.Add((WaitInstruction)item); break;
                case InstructionType.ConditionalBranchInstruction: item = new ConditionalBranchInstruction((ConditionalBranchInstruction)ins); ConditionalBranches.Add((ConditionalBranchInstruction)item); break;
                case InstructionType.JumpBranchInstruction: item = new JumpBranchInstruction((JumpBranchInstruction)ins); miscellaneouses.Add((JumpBranchInstruction)item); break;
                case InstructionType.MissionInstruction: item = new MissionInstruction((MissionInstruction)ins); miscellaneouses.Add((MissionInstruction)item); break;
            }
            instructions.Add(item);
        }
    }
    public DialogBranch(XmlNode rootNode) : this(int.Parse(rootNode["ID"].InnerText))
    {
        name = rootNode["Name"].InnerText;
        if (rootNode["ClusterName"] != null)
            ClusterName = rootNode["ClusterName"].InnerText;
        if (rootNode["IsStartBranch"] != null)
            IsStartBranch = bool.Parse(rootNode["IsStartBranch"].InnerText);

        IDCounter = int.Parse(rootNode["IDCounter"].InnerText);
        XmlNodeList instructions = rootNode.SelectNodes("Instruction");
        foreach (XmlNode instruction in instructions)
        {
            InstructionType type = (InstructionType)Enum.Parse(typeof(InstructionType), instruction["Type"].InnerText);

            switch (type)
            {
                default: Debug.Log("Deserialization: Type Error!!!!"); break;

                case InstructionType.Dialog: SetDialog(instruction); break;
                case InstructionType.DialogBranch: SetDialogBranch(instruction); break;
                case InstructionType.ChoiceBody: SetChoiceBody(instruction); break;
                case InstructionType.Background: SetBackgroundInstruction(instruction); break;
                case InstructionType.Music: SetMisc(instruction); break;
                case InstructionType.Minigame: SetMisc(instruction); break;
                case InstructionType.ConditionSwitchInstruction: SetConditionIns(instruction); break;
                case InstructionType.ConditionVariableInstruction: SetConditionIns(instruction); break;
                case InstructionType.ConditionalBranchInstruction: SetConditionalBranch(instruction); break;
                case InstructionType.WaitInstruction: SetWaitIns(instruction); break;
                case InstructionType.JumpBranchInstruction: SetJBIns(instruction); break;
                case InstructionType.MissionInstruction: SetMissionIns(instruction); break;
            }
        }

    }

    private void SetDialog(XmlNode currentNode)
    {
        Dialog dialog = new Dialog(currentNode);
        dialogs.Add(dialog);
        instructions.Add(dialog);
    }

    private void SetConditionIns(XmlNode currentNode)
    {
        ConditionInstruction instruction = new ConditionInstruction(currentNode);
        miscellaneouses.Add(instruction);
        Conditions.Add(instruction);
        instructions.Add(instruction);
    }

    private void SetChoiceBody(XmlNode currentNode)
    {
        ChoiceBody cb = new ChoiceBody(currentNode);
        choiceBodies.Add(cb);
        instructions.Add(cb);
    }

    private void SetDialogBranch(XmlNode currentNode)
    {
        DialogBranch branch = new DialogBranch(currentNode);
        instructions.Add(branch);
    }

    private void SetMisc(XmlNode currentNode)
    {
        Miscellaneous instruction = new Miscellaneous(currentNode);
        instructions.Add(instruction);
        miscellaneouses.Add(instruction);
    }

    private void SetWaitIns(XmlNode currentNode)
    {
        WaitInstruction instruction = new WaitInstruction(currentNode);
        instructions.Add(instruction);
        miscellaneouses.Add(instruction);
    }
    private void SetJBIns(XmlNode currentNode)
    {
        JumpBranchInstruction instruction = new JumpBranchInstruction(currentNode);
        instructions.Add(instruction);
        miscellaneouses.Add(instruction);
    }
    private void SetMissionIns(XmlNode currentNode)
    {
        MissionInstruction instruction = new MissionInstruction(currentNode);
        instructions.Add(instruction);
        miscellaneouses.Add(instruction);
    }
    private void SetBackgroundInstruction(XmlNode currentNode)
    {
        BackgroundInstruction instruction = new BackgroundInstruction(currentNode);
        instructions.Add(instruction);
        miscellaneouses.Add(instruction);
    }
    private void SetConditionalBranch(XmlNode currentNode)
    {
        ConditionalBranchInstruction instruction = new ConditionalBranchInstruction(currentNode);
        instructions.Add(instruction);
        ConditionalBranches.Add(instruction);
    }

    public int GetInstructionIndex(Instruction instruction)
    {
        int i = 0;
        foreach (Instruction item in instructions)
        {
            if (item.id == instruction.id)
                return i;
            i++;
        }

        Debug.LogError("Instruction index returned -1 in DialogBranch:GetInstructionIndex --> " + instruction.name);
        return -1;
    }
}
[System.Serializable]

public class ChoiceBody : Instruction
{
    [XmlElement("DialogText")] public string DialogText;

    [XmlElement("IsLoop")] public bool isLoop;

    [XmlElement("Choice")] public List<Choice> choices;

    [XmlIgnore] public int LastSelectedIndex;

    public ChoiceBody()
    {
        choices = new List<Choice>();
        type = InstructionType.ChoiceBody;
        DialogText = "";
        LastSelectedIndex = -1;
    }
    public ChoiceBody(int id) : this()
    {
        this.id = id;
    }

    public ChoiceBody(List<Choice> choices) : this()
    {
        this.choices = choices;
    }
    public ChoiceBody(ChoiceBody instruction) : this(0)
    {
        name = instruction.name;
        DialogText = instruction.DialogText;
        isLoop = instruction.isLoop;

        foreach (var item in instruction.choices)
        {
            Choice choice = new Choice(item);
            choices.Add(choice);
        }
    }

    public ChoiceBody(XmlNode choiceBodyRootNode) : this(int.Parse(choiceBodyRootNode["ID"].InnerText))
    {
        name = choiceBodyRootNode["Name"].InnerText;
        isLoop = bool.Parse(choiceBodyRootNode["IsLoop"].InnerText);
        if (choiceBodyRootNode["DialogText"] != null)
            DialogText = choiceBodyRootNode["DialogText"].InnerText;

        XmlNodeList choiceNodes = choiceBodyRootNode.SelectNodes("Choice");
        foreach (XmlNode node in choiceNodes)
        {
            Choice choice = new Choice(node);
            choices.Add(choice);
        }

    }
}
[System.Serializable]

public class Choice
{
    [XmlElement("ChoiceText")] public string choiceText;
    [XmlElement("IsSelectable")] public bool isVisible;
    [XmlElement("ChoicesToActivate")] public List<int> choicesToActivate;

    public bool hasSelected;
    public bool isSelectable;

    [XmlElement("Branch")][SerializeReference] public DialogBranch branch;

    public Choice()
    {
        isSelectable = true;
        branch = new DialogBranch();
        choicesToActivate = new List<int>();
    }

    public Choice(string choiceText, bool isVisible, List<int> choicesToActivate, bool hasSelected, bool isSelectable, DialogBranch branch)
    {
        this.choiceText = choiceText;
        this.isVisible = isVisible;
        this.choicesToActivate = choicesToActivate;
        this.hasSelected = hasSelected;
        this.isSelectable = isSelectable;
        this.branch = branch;
    }

    public Choice(Choice choice) : this()
    {
        choiceText = choice.choiceText;
        isVisible = choice.isVisible;
        hasSelected = choice.hasSelected;
        isSelectable = choice.isSelectable;
        foreach (int item in choice.choicesToActivate)
        {
            choicesToActivate.Add(item);
        }
        branch = new DialogBranch(choice.branch);
    }
    public Choice(XmlNode choiceRootNode) : this()
    {
        choiceText = choiceRootNode["ChoiceText"].InnerText;
        if (choiceRootNode["IsSelectable"].InnerText == "true")
            isVisible = true;
        else
            isVisible = false;
        XmlNode branchRootNode = choiceRootNode.SelectSingleNode("Branch");

        branch = new DialogBranch(branchRootNode);

        XmlNodeList choicesList = choiceRootNode.SelectNodes("ChoicesToActivate");
        foreach (XmlNode node in choicesList)
        {
            choicesToActivate.Add(int.Parse(node.InnerText));
        }

    }
}
[System.Serializable]

public class Dialog : Instruction
{
    [XmlElement("Expression")] public string Expression;
    [XmlElement("Animation")] public string Animation;
    [XmlElement("Voice")] public string voiceOverName;

    [XmlArray("Sentences")]
    [XmlArrayItem("s")]
    [TextArea(3,5)]
    public List<string> sentences;

    public Dialog()
    {
        type = InstructionType.Dialog;
        sentences = new List<string>();
        name = "NONE";
        Expression = "NONE";
        Animation = "NONE";
        voiceOverName = "NONE";
    }
    public Dialog(int id) : this()
    {
        this.id = id;
    }

    public Dialog(Dialog instruction) : base(instruction)
    {
        Expression = instruction.Expression;
        Animation = instruction.Animation;
        voiceOverName = instruction.voiceOverName;
        sentences = new List<string>();
        foreach (var item in instruction.sentences)
        {
            sentences.Add(new string(item));
        }
    }

    public Dialog(XmlNode currentNode)
    {
        type = InstructionType.Dialog;
        sentences = new List<string>();

        name = currentNode["Name"].InnerText;
        Expression = currentNode["Expression"].InnerText;
        Animation = currentNode["Animation"].InnerText;
        voiceOverName = currentNode["Voice"].InnerText;
        id = int.Parse(currentNode["ID"].InnerText);

        XmlNode sentenceNode = currentNode.SelectSingleNode("Sentences");
        XmlNode currentSentenceNode = sentenceNode.FirstChild;
        while (currentSentenceNode != null)
        {
            sentences.Add(currentSentenceNode.InnerText);
            currentSentenceNode = currentSentenceNode.NextSibling;
        }
    }

}

[System.Serializable]
public class ConditionInstruction : Miscellaneous
{
    [XmlElement("Condition")] public Condition condition;
    [XmlElement("Operation")] public ConditionOperation operation;
    [XmlElement("Operand")] public ConditionOperand operand;
    [XmlElement("Operand_VariableName")] public string operand_VariableName;
    [XmlElement("RandomValueStart")] public int randomValueStart;
    [XmlElement("RandomValueEnd")] public int randomValueEnd;

    public ConditionInstruction() { }
    public ConditionInstruction(int id) : base()
    {
        this.id = id;
        operand_VariableName = "";
        operation = ConditionOperation.Set;
        operand = ConditionOperand.Constant;
        randomValueEnd = 0;
        randomValueStart = 0;
    }

    public ConditionInstruction(Condition condition, ConditionOperation operation, int id) : this(id)
    {
        this.name = condition.conditionName;
        this.condition = condition;
        this.operation = operation;
        type = (InstructionType)Enum.Parse(typeof(InstructionType), "Condition" + condition.type + "Instruction");
    }

    public ConditionInstruction(Condition condition, ConditionOperation operation, ConditionOperand operand, int id) : this(id)
    {
        this.name = condition.conditionName;
        this.condition = condition;
        this.operation = operation;
        this.operand = operand;
        type = (InstructionType)Enum.Parse(typeof(InstructionType), "Condition" + condition.type + "Instruction");
    }

    public ConditionInstruction(XmlNode node) : base(node)
    {
        condition = new Condition(node.SelectSingleNode("Condition"));
        operation = (ConditionOperation)Enum.Parse(typeof(ConditionOperation), node["Operation"].InnerText);
        operand = (ConditionOperand)Enum.Parse(typeof(ConditionOperand), node["Operand"].InnerText);
        operand_VariableName = node["Operand_VariableName"].InnerText;
        randomValueStart = int.Parse(node["RandomValueStart"].InnerText);
        randomValueEnd = int.Parse(node["RandomValueEnd"].InnerText);
    }

    public ConditionInstruction(ConditionInstruction ins)
    {
        name = ins.name;
        type = ins.type;
        condition = ins.condition;
        operation = ins.operation;
        operand = ins.operand;
        operand_VariableName = ins.operand_VariableName;
        randomValueStart = ins.randomValueStart;
        randomValueEnd = ins.randomValueEnd;

    }

    public enum ConditionOperation
    {
        [XmlEnum("Set")] Set,
        [XmlEnum("Add")] Add,
        [XmlEnum("Sub")] Sub,
        [XmlEnum("Mul")] Mul,
        [XmlEnum("Div")] Div,
        [XmlEnum("Mod")] Mod,
    }

    public enum ConditionOperand
    {
        [XmlEnum("Constant")] Constant,
        [XmlEnum("Variable")] Variable,
        [XmlEnum("Random")] Random
    }
}

[System.Serializable]
public class WaitInstruction : Miscellaneous
{
    [XmlElement("WaitTime")] public float waitTime;

    public WaitInstruction() { }
    public WaitInstruction(int id) : base()
    {
        type = InstructionType.WaitInstruction;
        this.id = id;
    }

    public WaitInstruction(float waitTime, int id) : this(id)
    {
        name = "Wait : " + waitTime + " seconds";
        this.waitTime = waitTime;
    }

    public WaitInstruction(XmlNode node) : base(node)
    {
        waitTime = float.Parse(node["WaitTime"].InnerText);
        name = "Wait : " + waitTime + " seconds";
    }

    public WaitInstruction(WaitInstruction ins)
    {
        name = ins.name;
        type = ins.type;
        waitTime = ins.waitTime;

    }
}
[System.Serializable]
public class ConditionalBranchInstruction : Instruction
{
    [XmlElement("Condition")] public List<Condition> conditions;
    [XmlElement("CheckSign")] public List<Condition.CheckSign> CheckSigns;
    [XmlElement("Logic")] public List<Logic> logics;
    [XmlElement("DialogBranch")][SerializeReference] public DialogBranch branch;
    [XmlElement("ElseDialogBranch")][SerializeReference] public DialogBranch elseBranch;
    [XmlElement("ElseBranchCreated")] public bool ElseBranchCreated;

    public ConditionalBranchInstruction() { }
    public ConditionalBranchInstruction(int id)
    {
        name = "Conditional Branch";
        type = InstructionType.ConditionalBranchInstruction;
        this.id = id;
        conditions = new List<Condition>();
        logics = new List<Logic>();
        CheckSigns = new List<Condition.CheckSign>();
        branch = new DialogBranch(0);
        elseBranch = new DialogBranch(1);
    }

    public ConditionalBranchInstruction(ConditionalBranchInstruction ins) : this(0)
    {
        foreach (var item in ins.conditions)
        {
            conditions.Add(item);
        }
        foreach (var item in ins.logics)
        {
            logics.Add(item);
        }
        foreach (var item in ins.CheckSigns)
        {
            CheckSigns.Add(item);
        }
        branch = new DialogBranch(ins.branch);
        elseBranch = new DialogBranch(ins.elseBranch);
        ElseBranchCreated = ins.ElseBranchCreated;
    }

    public ConditionalBranchInstruction(List<Condition> conditions, List<Logic> logic, DialogBranch branch, DialogBranch elseBranch, int id) : this(id)
    {
        this.conditions = conditions;
        this.logics = logic;
        this.branch = branch;
        this.elseBranch = elseBranch;
    }

    public ConditionalBranchInstruction(XmlNode node) : this(int.Parse(node["ID"].InnerText))
    {
        XmlNodeList conditionNodes = node.SelectNodes("Condition");
        foreach (XmlNode conditionNode in conditionNodes)
        {
            Condition condition = new Condition(conditionNode);
            conditions.Add(condition);
        }

        XmlNodeList logicNodes = node.SelectNodes("Logic");
        foreach (XmlNode logicNode in logicNodes)
        {
            Logic logic = (Logic)Enum.Parse(typeof(Logic), logicNode.InnerText);
            logics.Add(logic);
        }

        XmlNodeList CheckSignNodes = node.SelectNodes("CheckSign");
        foreach (XmlNode checkSignNode in CheckSignNodes)
        {
            Condition.CheckSign sign = (Condition.CheckSign)Enum.Parse(typeof(Condition.CheckSign), checkSignNode.InnerText);
            CheckSigns.Add(sign);
        }

        branch = new DialogBranch(node.SelectSingleNode("DialogBranch"));
        elseBranch = new DialogBranch(node.SelectSingleNode("ElseDialogBranch"));
        ElseBranchCreated = bool.Parse(node.SelectSingleNode("ElseBranchCreated").InnerText);
    }

    public enum Logic
    {
        And,
        Or
    }
}

[System.Serializable]
public class JumpBranchInstruction : Miscellaneous
{

    [XmlElement("FileName")]
    public string FileName;

    [XmlElement("BranchName")]
    public string BranchName;

    [XmlElement("Return")]
    public bool Return;

    public JumpBranchInstruction() : base()
    {

    }

    public JumpBranchInstruction(int id)
    {
        type = InstructionType.JumpBranchInstruction;
        this.id = id;
    }

    public JumpBranchInstruction(string fileName, string branchName, bool @return, int id) : this(id)
    {
        FileName = fileName;
        BranchName = branchName;
        Return = @return;
    }

    public JumpBranchInstruction(XmlNode node) : base(node)
    {
        FileName = node["FileName"].InnerText;
        BranchName = node["BranchName"].InnerText;
        Return = bool.Parse(node["Return"].InnerText);
    }

    public JumpBranchInstruction(JumpBranchInstruction ins)
    {
        name = ins.name;
        type = ins.type;
        FileName = ins.FileName;
        BranchName = ins.BranchName;
        Return = ins.Return;
    }
}

[System.Serializable]
public class MissionInstruction : Miscellaneous
{
    [XmlElement("MissionName")] public string MissionName;
    [XmlElement("MissionInstructionType")] public MissionInstructionType missionInstructionType;

    [XmlElement("ObjectiveName")] public string ObjectiveName;
    [XmlElement("IncreaseAmount")] public float IncreaseAmount;

    public MissionInstruction()
    {
        type = InstructionType.MissionInstruction;
    }

    public MissionInstruction(int id) : this()
    {
        this.id = id;
    }

    public MissionInstruction(string missionName, MissionInstructionType missionInstructionType, string objectiveName, int id) : this(id)
    {
        MissionName = missionName;
        this.missionInstructionType = missionInstructionType;
        ObjectiveName = objectiveName;
    }

    public MissionInstruction(XmlNode node) : base(node)
    {
        MissionName = node["MissionName"].InnerText;
        ObjectiveName = node["ObjectiveName"].InnerText;
        missionInstructionType = (MissionInstructionType)Enum.Parse(typeof(MissionInstructionType), node["MissionInstructionType"].InnerText);
        if (node["IncreaseAmount"] != null)
            IncreaseAmount = float.Parse(node["IncreaseAmount"].InnerText);
        else
            IncreaseAmount = 0f;
    }

    public MissionInstruction(MissionInstruction ins)
    {
        name = ins.name;
        type = ins.type;
        MissionName = ins.MissionName;
        missionInstructionType = ins.missionInstructionType;
        ObjectiveName = ins.ObjectiveName;
    }

    public enum MissionInstructionType
    {
        [XmlEnum("Start")] Start,
        [XmlEnum("Complete")] Complete,
        [XmlEnum("StartObjective")] StartObjective,
        [XmlEnum("CompleteObjective")] CompleteObjective,
        [XmlEnum("StartNextObjective")] StartNextObjective,
        [XmlEnum("IncreaseObjectiveProgress")] IncreaseObjectiveProgress,
    }

}

[System.Serializable]
public class Miscellaneous : Instruction
{
    public Miscellaneous()
    {

    }

    public Miscellaneous(Instruction misc) : base(misc)
    {

    }
    public Miscellaneous(string name, InstructionType type, int id) : base(name, type, id)
    {

    }
    public Miscellaneous(XmlNode node) : base(node)
    {

    }
}

[System.Serializable]
public class BackgroundInstruction : Miscellaneous
{
    [XmlElement("CheckDialogs")] public bool checkDialogs;
    public BackgroundInstruction()
    {

    }

    public BackgroundInstruction(BackgroundInstruction ins) : base(ins)
    {
        checkDialogs = ins.checkDialogs;
    }
    public BackgroundInstruction(string name, InstructionType type, bool checkDialogs, int id) : base(name, type, id)
    {
        this.checkDialogs = checkDialogs;
    }
    public BackgroundInstruction(XmlNode node) : base(node)
    {
        if (node["CheckDialogs"] != null)
        {
            checkDialogs = bool.Parse(node["CheckDialogs"].InnerText);
        }
    }
}
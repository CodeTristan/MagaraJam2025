using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using static MissionInstruction;
using UnityEngine.InputSystem.XR;
using System.Text;
using System.Reflection;
using System.Linq;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [Header("UI")]
    [SerializeField] private GameObject MissionScrollRectContent;
    [SerializeField] private GameObject QuestListUI;
    [SerializeField] private GameObject QuestUIPrefab;
    [SerializeField] private GameObject QuestUI;
    [SerializeField] private TextMeshProUGUI QuestNameText;
    [SerializeField] private GameObject ObjectiveScrollRectContent;
    [SerializeField] private ObjectiveUIHolder QuestObjectivePrefab;
    [SerializeField] private Canvas QuestCanvas;

    private List<GameObject> questUIList;
    private List<GameObject> objectiveUIList;

    [Header("Manager")]
    public List<Mission> CompletedMissions;
    public List<Mission> allMissions;
    public List<Mission> activeMissions;

    public void Init() //Runs everytime when scene is changed to MainScene
    {
        instance = this;

        CompletedMissions = new List<Mission>();
        activeMissions = new List<Mission>();
        questUIList = new List<GameObject>();
        objectiveUIList = new List<GameObject>();

        allMissions = GameManager.instance.allMissions;

        CompletedMissions = allMissions.Where(m => m.isCompleted).ToList();
        activeMissions = allMissions.Where(m => m.isActive).ToList();


        QuestCanvas.enabled = false;
        QuestUI.SetActive(false);
        QuestListUI.SetActive(false);
    }

    public void ExecuteInstruction(MissionInstruction instruction)
    {
        switch (instruction.missionInstructionType)
        {
            case MissionInstructionType.Start:
                StartMission(instruction.MissionName);
                break;

            case MissionInstructionType.Complete:
                {
                    Mission mission = GetMissionByName(instruction.MissionName);
                    if(mission == null)
                    {
                        Debug.LogError("Mission not found!! --> " + instruction.MissionName);
                        return;
                    }
                    if(mission.isActive == false)
                    {
                        Debug.LogError("Mission is not active! Start it first! --> " + instruction.MissionName);
                        return;
                    }
                    CompleteMission(mission);
                    break;
                }

            case MissionInstructionType.StartObjective:
                StartObjective(instruction.MissionName, instruction.ObjectiveName);
                CheckStartObjective(instruction.MissionName, instruction.ObjectiveName);
                break;

            case MissionInstructionType.CompleteObjective:
                CompleteObjective(instruction.MissionName, instruction.ObjectiveName);
                break;

            case MissionInstructionType.StartNextObjective:
                {
                    Mission mission = GetMissionByName(instruction.MissionName);
                    if (mission == null)
                    {
                        Debug.LogError("Mission not found!! --> " + instruction.MissionName);
                        return;
                    }
                    UpdateMission(mission);
                    break;
                }
            case MissionInstructionType.IncreaseObjectiveProgress:
                {
                    Mission mission = GetMissionByName(instruction.MissionName);
                    if (mission == null)
                    {
                        Debug.LogError("Mission not found!! --> " + instruction.MissionName);
                        return;
                    }
                    IncreaseObjectiveProgress(mission,instruction.ObjectiveName, instruction.IncreaseAmount);
                    break;
                }

            default:
                break;
        }
    }
    public void StartMission(string name)
    {
        Mission mission = GetMissionByName(name);
        if (mission == null)
            return;

        if (mission.isActive || mission.isCompleted) return;

        mission.StartMission();
        activeMissions.Add(mission);
        CheckStart(name);
    }

    public void StartObjective(string MissionName,string ObjectiveName)
    {
        Mission mission = GetMissionByName(MissionName);
        if (mission == null)
        {
            Debug.LogError("Mission not found!! --> " + MissionName);
            return;
        }
        Objective objective = null;
        foreach (var item in mission.objectives)
        {
            if(item.name == ObjectiveName)
            {
                objective = item;
                break;
            }
        }
        if(objective == null)
        {
            Debug.LogError("Objective not found!! --> Mission: " + MissionName + " --> Objective: " + ObjectiveName);
            return;
        }
        if(mission.isActive == false)
        {
            Debug.LogError("Mission hasn't started!! Start The Mission First --> " + mission.name);
            return;
        }
        mission.StartObjective(objective);
    }
    public void CompleteObjective(string MissionName, string ObjectiveName)
    {
        Mission mission = GetMissionByName(MissionName);
        if (mission == null)
        {
            Debug.LogError("Mission not found!! --> " + MissionName);
            return;
        }
        Objective objective = null;
        foreach (var item in mission.objectives)
        {
            if (item.name == ObjectiveName)
            {
                objective = item;
                break;
            }
        }
        if (objective == null)
        {
            Debug.LogError("Objective not found!! --> Mission: " + MissionName + " --> Objective: " + ObjectiveName);
            return;
        }
        if(objective.isVisible == false)
        {
            Debug.LogError("Objective hasn't started yet!! Start the objective first! --> Mission: " + MissionName + " --> Objective: " + ObjectiveName);
            return;
        }
        if(objective.isCompleted == true)
        {
            Debug.LogError("Objective has already completed!! --> Mission: " + MissionName + " --> Objective: " + ObjectiveName);
            return;
        }


        mission.CompleteObjective(objective);

        //Check if all missions are completed
        bool missionCompleted = true;
        foreach (var item in mission.objectives)
        {
            if (item.isCompleted == false)
            {
                missionCompleted = false;
                break;
            }
        }

        if (missionCompleted) CompleteMission(mission);

    }
    public bool UpdateMission(Mission mission)
    {
        bool returnVal = mission.StartNextObjective();
        CheckStartObjective(mission.name, mission.objectives[mission.objectiveIndex - 1].name);

        return returnVal;
    }
    public void CompleteMission(Mission mission)
    {
        mission.CompleteMission();
        activeMissions.Remove(mission);
        CompletedMissions.Add(mission);
    }

    public void IncreaseObjectiveProgress(Mission mission,string objectiveName, float amount)
    {
        CollectibleObjective collectibleObjective = null;
        foreach (var item in mission.objectives)
        {
            if (objectiveName == item.name)
            {
                collectibleObjective = item as CollectibleObjective;
                break;
            }
        }
        if (collectibleObjective == null)
        {
            Debug.LogError("Objective is not in this mission!! Mission: " + name + " --> Objective: " + collectibleObjective.name);
            return;
        }

        collectibleObjective.AddProgress(amount);
        if (!mission.isActive) //If mission is not active, dont Show popups yet.
            return;

        if (collectibleObjective.CurrentProgress >= collectibleObjective.MaxProgress)
        {
            CompleteObjective(mission.name,collectibleObjective.name);
            if (!UpdateMission(mission))
            {
                bool completeQuest = true;
                foreach (var item in mission.objectives)
                {
                    completeQuest &= item.isCompleted;
                }
                if (completeQuest)
                    CompleteMission(mission);
            }

        }
    }

    public void ToggleQuestUI(InputAction.CallbackContext context)
    {
        if(QuestListUI.activeSelf)
        {
            _CloseQuestListUI();
            _CloseQuestUI();
        }
        else
        {
            _OpenQuestListUI();
        }
    }
    public void _OpenQuestListUI()
    {
        MusicManager.instance.PlaySound("QuestUI");
        QuestCanvas.enabled = true;
        QuestListUI.SetActive(true);
        _ShowQuests(0);
        BackgroundManager.instance.ToggleGeneralCanvas(false);
    }

    public void _CloseQuestListUI()
    {
        if (QuestCanvas.enabled == false)
            return;
        MusicManager.instance.PlaySound("QuestUI");
        QuestCanvas.enabled = false;
        QuestListUI.SetActive(false);
        QuestUI.SetActive(false);
        BackgroundManager.instance.ToggleGeneralCanvas(true);
    }
    public void _ShowQuest(Mission mission)
    {
        foreach (GameObject item in objectiveUIList)
        {
            Destroy(item.gameObject);
        }
        objectiveUIList.Clear();

        QuestNameText.text = mission.displayName;
        foreach (var item in mission.objectives)
        {
            if(item.isVisible == false)
                continue;
            ObjectiveUIHolder obj = Instantiate(QuestObjectivePrefab, ObjectiveScrollRectContent.transform);
            obj.toggle.isOn = item.isCompleted;
            obj.objectiveNameText.text = item.name;
            obj.objectiveDescriptionText.text = item.description;
            obj.objective = item;


            if(item is CollectibleObjective collectible)
            {                 
                obj.objectiveProgressText.text = collectible.CurrentProgress + "/" + collectible.MaxProgress;
            }
            else
            {
                obj.objectiveProgressText.text = "";
            }

            obj.gameObject.SetActive(true);
            objectiveUIList.Add(obj.gameObject);
        }

        QuestUI.SetActive(true);
        
    }

    public void QuestHelpQuestion()
    {
        ChoiceBody choiceBody = new ChoiceBody();
        List<ChoiceManager.ChoiceFunction> functions = new List<ChoiceManager.ChoiceFunction>();

        foreach (var mission in allMissions)
        {
            if (mission.activeObjectives[0].helpText == "" || mission.isActive == false)
                continue;

            choiceBody.choices.Add(new Choice(mission.displayName, true, null, false, true, null));
            functions.Add(new ChoiceManager.ChoiceFunction(QuestHelp));
        }

        choiceBody.choices.Add(new Choice("Leave", true, null, false, true, null));
        functions.Add(new ChoiceManager.ChoiceFunction(ChoiceManager.instance.ConfirmChoiceEvent));

        ChoiceManager.instance.StartChoiceWithEvent(choiceBody, functions);
    }

    public void QuestHelp(int choiceIndex)
    {
        ChoiceManager.instance.ConfirmChoiceEvent(choiceIndex);

        Mission mission = allMissions[choiceIndex];
        Debug.Log(mission.activeObjectives[0].helpText);

        Dialog dialog = new Dialog();
        dialog.sentences.Add(mission.activeObjectives[0].helpText);
        dialog.name = "Keeper";
        dialog.Expression = "NoSprite";
        
        DialogBranch br = new DialogBranch();
        br.instructions.Add(dialog);

        DialogManager.instance.ResetDialogManager();
        DialogManager.instance.StartBranch(br);
    }

    public void _CloseQuestUI()
    {
        MusicManager.instance.PlaySound("QuestUI");
        QuestUI.SetActive(false);
    }
    public void _ShowQuests(int index)
    {
        foreach (GameObject item in questUIList)
        {
            DestroyImmediate(item.gameObject);
        }
        questUIList.Clear();
        
        switch (index)
        {
            case 0:
                {
                    int i = 0;
                    foreach (Mission mission in allMissions)
                    {
                        if (mission.isActive == false) continue;

                        MissionUIHolder obj = Instantiate(QuestUIPrefab, MissionScrollRectContent.transform).GetComponent<MissionUIHolder>();
                        obj.mission = mission;
                        obj.button.onClick.AddListener(() => _ShowQuest(mission));
                        obj.text.text = mission.displayName;
                        obj.rectTransform.SetSiblingIndex(++i);
                        obj.gameObject.SetActive(true);
                        questUIList.Add(obj.gameObject);
                    }
                    break;
                }
            case 1:
                {
                    int i = 1;
                    foreach (Mission mission in allMissions)
                    {
                        if (mission.type != MissionType.Main || mission.isActive == false)
                            continue;
                        MissionUIHolder obj = Instantiate(QuestUIPrefab, MissionScrollRectContent.transform).GetComponent<MissionUIHolder>();
                        obj.mission = mission;
                        obj.button.onClick.AddListener(() => _ShowQuest(mission));
                        obj.text.text = mission.displayName;
                        obj.rectTransform.SetSiblingIndex(++i);
                        obj.gameObject.SetActive(true);
                        questUIList.Add(obj.gameObject);

                    }
                    break;
                }
            case 2:
                {
                    int i = 2;
                    foreach (Mission mission in allMissions)
                    {
                        if (mission.type != MissionType.Side || mission.isActive == false)
                            continue;
                        MissionUIHolder obj = Instantiate(QuestUIPrefab, MissionScrollRectContent.transform).GetComponent<MissionUIHolder>();
                        obj.mission = mission;
                        obj.button.onClick.AddListener(() => _ShowQuest(mission));
                        obj.text.text = mission.displayName;
                        obj.rectTransform.SetSiblingIndex(++i);
                        obj.gameObject.SetActive(true);
                        questUIList.Add(obj.gameObject);

                    }
                    break;
                }
            default: break;
        }
    }
    
    private Mission GetMissionByName(string name)
    {
        foreach (var mission in allMissions)
        {
            if(mission.name == name) 
                return mission;
        }

        Debug.LogError("Mission couldn't found in MissionManager:GetMissionByName --> " + name);
        return null;
    }


    //TEMPORARY SOLUTION CHANGE LATER
    public void CheckStart(string missionName)
    {
        switch (missionName)
        {

            default:
                break;
        }
    }

    private void CheckStartObjective(string missionName,string objectiveName)
    {
        string tag = missionName + "_" + objectiveName;

        switch (tag)
        {

            default: break;

        }
    }
}

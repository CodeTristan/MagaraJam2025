
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool DebugMod;

    [SerializeField] private SahneManager sahneManager;
    [SerializeField] private XMLManager XMLmanager;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ConditionManager conditionManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private BackgroundManager backgroundManager;
    [SerializeField] private CanvasManager canvasManager;

    [SerializeField] private LittleGameManager littleGameManager;



    public AllPlacesSO allPlaces;
    public AllConditions allConditions;
    public List<Mission> allMissions;
    public AllBackgroundDialogsSO allBackgroundDialogs;
    public AllItemsSO allItems;



    [Header("Debug")]
    [SerializeField] private DebugTestScript debugTestScript;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        allConditions = new AllConditions();
        allMissions = new List<Mission>();
        allPlaces.Init();
        allBackgroundDialogs.Init();
        allItems.Init();

        XMLmanager.Init();
        inputManager.Init();
        sahneManager.Init();


        dialogManager.Init();  //Dialogs, choices and mission are set in here.
        conditionManager.Init();  //Conditions from editor are set in here.
        musicManager.Init();

        backgroundManager.Init();


        if (DebugMod)
        {
            debugTestScript.Init(); 
        }


        canvasManager.Init();
        //sahneManager.LoadScene("MainMenu");

        littleGameManager.Init();

        List<Condition> conditions = new List<Condition> { new CheckCondition(ConditionNameGroup.Game, GameConditionName.GameStart, 1) };
        if (ConditionManager.instance.CheckConditions(conditions, false, false))
        {
            DialogManager.instance.StartBranch(DialogManager.instance.FirstBranch);
            ConditionManager.instance.ChangeCondition(GameConditionName.GameStart, 0);
        }

        Debug.Log("Initialize Completed!");
    }

   
}
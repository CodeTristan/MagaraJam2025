using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//[System.Serializable]
//public class AllBackgroundDialogs
//{
//    public AllBackgroundDialogsSO AllBackgroundDialogData;

//    private Dictionary<(BackgroundName,BackgroundCharacterName),BackgroundDialog> backgroundDialogDict;

//    public BackgroundDialog GetBackgroundDialogByName(BackgroundName backgroundName,BackgroundCharacterName characterName)
//    {
//        backgroundDialogDict.TryGetValue((backgroundName, characterName), out BackgroundDialog dialog);

//        if (dialog != null)
//        {
//            return dialog;
//        }
//        else
//        {
//            Debug.LogError($"Background dialog not found for {backgroundName} and {characterName}");
//            return null;
//        }
//    }

//    public List<BackgroundDialog> GetAllBackgroundDialogs()
//    {
//        return backgroundDialogDict.Values.ToList();
//    }

//    public AllBackgroundDialogs()
//    {
//        backgroundDialogDict = AllBackgroundDialogData.backgroundDialogs.ToDictionary(x => (x.backgroundName, x.characterName), x => new BackgroundDialog(x));

//        //backgroundDialogs = new List<BackgroundDialog>();

//        //#region Yan Ýþler
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Tavern, BackgroundCharacterName.Tavernkeep,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "TavernFirstDay", "StartDialogs", false,
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                )
//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Ceremonial_Ground, BackgroundCharacterName.Keeper,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Question", "StartDialogs", true,
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                )
//        //            },
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.Action_Point,2)
//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Lyceum_Apathe, BackgroundCharacterName.Bullshiticus,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Lyceum Random", "Jobs", true,
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    true
//        //                )
//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Great_Library_Apathe, BackgroundCharacterName.Thesauros,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Library Random", "Jobs", true,
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    true
//        //                )
//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    );

//        //#endregion

//        //#region Zeus
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Temple_Of_Zeus, BackgroundCharacterName.Zeus,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Zeus1-NotFinished", "Zeus", true,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.ZeusQuest1_1, 1),
//        //                        new CheckCondition(ConditionName.Charisma,2, Condition.ConditionType.Variable,"<")
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                ),
//        //                new DialogTrigger
//        //                (
//        //                    "Zeus2", "Zeus", false,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.Charisma, 2, Condition.ConditionType.Variable, ">=")
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.Hercules_Active,1),
//        //                        new CheckCondition(ConditionName.ZeusQuest1_1,0),
//        //                        new CheckCondition(ConditionName.ZeusQuest1_2,1)
//        //                    }
//        //                ),
//        //                new DialogTrigger
//        //                (
//        //                    "Zeus-EndDemo", "Zeus", true,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.ZeusQuest1_3,1),
//        //                        new CheckCondition(ConditionName.Charisma, 1000, Condition.ConditionType.Variable, "<")
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                ),
//        //            },
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.Zeus_Active,1)
//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Temple_Of_Zeus, BackgroundCharacterName.Hercules,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Zeus2 Hercules", "Zeus", true,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.ZeusQuest1_2,1),
//        //                        new CheckCondition(ConditionName.Zeus1_MinigameFinish,0)
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                )
//        //            },
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.Hercules_Active,1)
//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Zeus_Forest, BackgroundCharacterName.Hippolyta,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Zeus2 ZeusForest1", "Zeus", false,
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                )
//        //            },
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.Hercules_Active,1)
//        //            }
//        //        )
//        //    );

//        //#endregion

//        //#region Athena
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Temple_Of_Athena, BackgroundCharacterName.Athena,
//        //            new List<DialogTrigger>()
//        //            {
//        //                new DialogTrigger
//        //                (
//        //                    "Athena1 NotFinished", "Athena", true,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.Athena_Start,1),
//        //                        new CheckCondition(ConditionName.Wisdom,2, Condition.ConditionType.Variable,"<")
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    true
//        //                ),
//        //                new DialogTrigger
//        //                (
//        //                    "Athena Quest1", "Athena", false,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.Wisdom,2, Condition.ConditionType.Variable,">=")
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.AthenaQuest1,1)
//        //                    }
//        //                ),
//        //                new DialogTrigger
//        //                (
//        //                    "Athena1 BeforeMaze", "Athena", true,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.AthenaQuest1_MazeDone,0),
//        //                        new CheckCondition(ConditionName.AthenaQuest1,1),

//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    },
//        //                    true
//        //                ),
//        //                new DialogTrigger
//        //                (
//        //                    "Athena1 End", "Athena", false,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.AthenaQuest1_MazeDone,1)
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.AthenaQuest2,1),
//        //                        new CheckCondition(ConditionName.AthenaQuest1,0),
//        //                        new CheckCondition(ConditionName.AthenaQuest1_MazeDone,0)
//        //                    }
//        //                ),
//        //                new DialogTrigger
//        //                (
//        //                    "Athena-DemoEnd", "Athena", true,
//        //                    new List<CheckCondition>()
//        //                    {
//        //                        new CheckCondition(ConditionName.AthenaQuest2,1),
//        //                        new CheckCondition(ConditionName.Wisdom,1000, Condition.ConditionType.Variable,"<")
//        //                    },
//        //                    new List<CheckCondition>()
//        //                    {

//        //                    }
//        //                ),
//        //            },
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.Athena_Active,1)
//        //            }
//        //        )
//        //    );

//        //#endregion

//        //#region Others
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Temple_Of_Hera, BackgroundCharacterName.Hera,
//        //            new List<DialogTrigger>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Temple_Of_Artemis, BackgroundCharacterName.Artemis,
//        //            new List<DialogTrigger>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    );
//        //backgroundDialogs.Add
//        //    (
//        //        new BackgroundDialog
//        //        (
//        //            BackgroundName.Temple_Of_Aphrodite, BackgroundCharacterName.Aphrodite,
//        //            new List<DialogTrigger>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    );

//        //#endregion
//    }


//}

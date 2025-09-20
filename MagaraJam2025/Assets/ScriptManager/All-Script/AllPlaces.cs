using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//    [System.Serializable]
//public class AllPlaces
//{
//    private AllPlacesSO allPlacesSO;

//    private Dictionary<string,PlaceName> placeNameDict;
//    private Dictionary<PlaceName, Place> placeDict;

//    public PlaceName GetPlaceNameEnum(string name)
//    {
//        if(placeNameDict.TryGetValue(name, out var outValue))
//            return outValue;


//        Debug.LogError("Place not found!! --> " + name);
//        return PlaceName.Null;
//    }

//    public Place GetPlaceByName(PlaceName placeName)
//    {
//        if(placeName != PlaceName.Null && placeDict.TryGetValue(placeName, out var place))
//        {
//            return place;
//        }

//        Debug.LogError("Place error! No place in MapManager called --> " + placeName);
//        return null;
//    }

//    public List<Place> GetAllPlaces()
//    {
//        return placeDict.Values.ToList();
//    }
    
//    public AllPlaces()
//    {

//        placeNameDict = new Dictionary<string, PlaceName>();
//        placeDict = new Dictionary<PlaceName, Place>();

//        #region Dictionary
//        placeNameDict.Add("Tavern",PlaceName.Tavern);
//        placeNameDict.Add("Ceremonial Ground",PlaceName.Ceremonial_Ground);
//        placeNameDict.Add("Temple of Zeus",PlaceName.Temple_Of_Zeus);
//        placeNameDict.Add("Zeus Forest",PlaceName.Zeus_Forest);
//        placeNameDict.Add("Zeus Forest Naked",PlaceName.Zeus_Forest_Naked);
//        placeNameDict.Add("Temple of Athena",PlaceName.Temple_Of_Athena);
//        placeNameDict.Add("Athena's Maze",PlaceName.Athenas_Maze);
//        placeNameDict.Add("Athena's Maze Inside",PlaceName.Athenas_Maze_Inside);
//        placeNameDict.Add("Your House",PlaceName.Your_House);
//        placeNameDict.Add("Lyceum (Apathe)",PlaceName.Lyceum_Apathe);
//        placeNameDict.Add("Great Library (Apathe)",PlaceName.Great_Library_Apathe);
//        placeNameDict.Add("Athena's Garden",PlaceName.Athenas_Garden);
//        #endregion

//        placeDict = allPlacesSO.places.ToDictionary(place => place.placeName, place => new Place(place));



//        //#region Yan iþler
//        //places.Add(new Place
//        //    (
//        //    PlaceName.Tavern, BackgroundName.Tavern,"Tavern",
//        //    new List<DialogTrigger>()
//        //    {

//        //    }

//        //    ));
//        //places.Add(new Place
//        //    (
//        //    PlaceName.Ceremonial_Ground, BackgroundName.Ceremonial_Ground,"Ceremony Ground",
//        //    new List<DialogTrigger>()
//        //    {
//        //        new DialogTrigger
//        //        (
//        //            "CeremonyFirstTime", "StartDialogs", false,
//        //            new List<CheckCondition>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    }
//        //    ));

//        //places.Add(new Place(
//        //    PlaceName.Your_House, BackgroundName.Your_House, "General Theme",
//        //    new List<DialogTrigger>()
//        //    { }, false));
//        //places.Add(new Place(
//        //    PlaceName.Lyceum_Apathe, BackgroundName.Lyceum_Apathe, "Job Theme",
//        //    new List<DialogTrigger>()
//        //    {
//        //        new DialogTrigger
//        //        (
//        //            "Lyceum Start", "Jobs", false,
//        //            new List<CheckCondition>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    }));
//        //places.Add(new Place(
//        //    PlaceName.Great_Library_Apathe, BackgroundName.Great_Library_Apathe, "Job Theme",
//        //    new List<DialogTrigger>()
//        //    {
//        //        new DialogTrigger
//        //        (
//        //            "Library Start", "Jobs", false,
//        //            new List<CheckCondition>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    }));

//        //#endregion

//        ///*------ZEUS------*/

//        //#region ----Zeus----

//        //#region Temple of Zeus
//        //places.Add(new Place(
//        //    PlaceName.Temple_Of_Zeus, BackgroundName.Temple_Of_Zeus,"Zeus Theme",
//        //    new List<DialogTrigger>()
//        //    {
//        //    new DialogTrigger
//        //    (
//        //        "Zeus1", "Zeus", false,
//        //        new List<CheckCondition>()
//        //        {

//        //        },
//        //        new List<CheckCondition>()
//        //        {
//        //            new CheckCondition(ConditionName.ZeusQuest1_1, 1)
//        //        }
//        //    ),
//        //    new DialogTrigger
//        //    (
//        //        "Zeus2 TempleAfterMinigame Without Zeus", "Zeus", true,
//        //        new List<CheckCondition>()
//        //        {
//        //            new CheckCondition(ConditionName.ZeusQuest1_2, 1),
//        //            new CheckCondition(ConditionName.Zeus1_MinigameFinish, 1),
//        //            new CheckCondition(ConditionName.Zeus_Active, 0)
//        //        },
//        //        new List<CheckCondition>()
//        //        {

//        //        }
//        //    ),
//        //    new DialogTrigger
//        //    (
//        //        "Zeus2 TempleAfterMinigame", "Zeus", false,
//        //        new List<CheckCondition>()
//        //        {
//        //            new CheckCondition(ConditionName.ZeusQuest1_2, 1),
//        //            new CheckCondition(ConditionName.Zeus1_MinigameFinish, 1),
//        //            new CheckCondition(ConditionName.Zeus_Active,1)
//        //        },
//        //        new List<CheckCondition>()
//        //        {
//        //            new CheckCondition(ConditionName.ZeusQuest1_2,0),
//        //            new CheckCondition(ConditionName.ZeusQuest1_3,1)
//        //        }
//        //    )
//        //    }
//        //    ));
//        //#endregion

//        //#region Zeus Forest
//        //places.Add(new Place(
//        //    PlaceName.Zeus_Forest, BackgroundName.Zeus_Forest, "NONE",
//        //    new List<DialogTrigger>()
//        //    {

//        //    }
//        //    ));
//        //#endregion

//        //#region Zeus Forest Naked
//        //places.Add(new Place(
//        //    PlaceName.Zeus_Forest_Naked, BackgroundName.Zeus_Forest_Naked, "NONE",
//        //    new List<DialogTrigger>()
//        //    {
//        //        new DialogTrigger
//        //        (
//        //            "Zeus2 AfterMinigame", "Zeus", false,
//        //            new List<CheckCondition>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    }
//        //    ));
//        //#endregion

//        //#endregion

//        ///*------ZEUS------*/


//        //#region Athena
//        //places.Add(new Place(
//        //    PlaceName.Temple_Of_Athena, BackgroundName.Temple_Of_Athena, "Athena Theme",
//        //    new List<DialogTrigger>()
//        //    {
//        //        new DialogTrigger
//        //        (
//        //            "Start", "Athena", false,
//        //            new List<CheckCondition>()
//        //            {

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    }
//        //    ));
//        //places.Add(new Place(
//        //    PlaceName.Athenas_Maze, BackgroundName.Athenas_Maze, "Athena Maze",
//        //    new List<DialogTrigger>()
//        //    {
//        //        new DialogTrigger
//        //        (
//        //            "Athena1 Maze", "Athena", true,
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.AthenaQuest1,1),
//        //                new CheckCondition(ConditionName.AthenaQuest1_MazeDone,0)
//        //            },
//        //            new List<CheckCondition>()
//        //            {
//        //            }
//        //        ),
//        //        new DialogTrigger
//        //        (
//        //            "Maze Entry", "Athena's Maze", true,
//        //            new List<CheckCondition>()
//        //            {
//        //                new CheckCondition(ConditionName.AthenaQuest1,0),
//        //                new Day(Day.DayEnum.Tuesday,"!="),
//        //                new Day(Day.DayEnum.Friday,"!=")

//        //            },
//        //            new List<CheckCondition>()
//        //            {

//        //            }
//        //        )
//        //    }
//        //    ));
//        //places.Add(new Place(
//        //    PlaceName.Athenas_Maze_Inside,BackgroundName.Athenas_Maze_Inside, "Athena Maze",
//        //    new List<DialogTrigger>()
//        //    {

//        //    }
//        //    ));

//        //#endregion
//    }

//}

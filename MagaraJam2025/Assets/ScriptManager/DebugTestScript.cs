using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTestScript : MonoBehaviour
{
    public List<CheckCondition> conditions;
    public List<PlaceName> placesToUnlock;
    public List<Item> itemsToAdd;
    public List<string> MissionsToStart;

    public bool MaxOutGodStats = false;
    public bool OpenAllPlaces = false;
    public bool MaxOutReputations = false;
    public bool LogConditionNames = false;


    private bool placesUnlocked = false;
    private bool MissionsUnlocked = false;
    public void Init()
    {

        if (OpenAllPlaces)
        {
            placesToUnlock.Clear();
            foreach (var place in GameManager.instance.allPlaces.GetAllPlaces())
            {
                if (!place.isLocked && place.PlaceData.placeName != PlaceName.Null)
                    continue;
                placesToUnlock.Add(place.PlaceData.placeName);
            }
        }

        foreach (var item in itemsToAdd)
        {
            GameManager.instance.allItems.AddItem(item);
        }


        SceneManager.sceneLoaded += UnlockPlaces;
        SceneManager.sceneLoaded += UnlockMissions;
        foreach (var item in conditions)
        {
            ConditionManager.instance.ChangeCondition(item.ToEnum(), item.status);
        }

        if (LogConditionNames)
        {
            List<Condition> conditionNames = new List<Condition>();
            foreach (var item in ConditionManager.instance.conditionDictionary.Values)
            {
                conditionNames.Add(item);
            }

            conditionNames.Sort((x, y) => x.conditionName.CompareTo(y.conditionName));

            string path = Directory.GetCurrentDirectory() + "/Debug";
            StreamWriter writer = new StreamWriter(path + "/ConditionNames.txt", false);
            foreach (var item in conditionNames)
            {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
            Debug.Log("Condition names written to " + path + "/ConditionNames.txt");
        }



    }

    private void UnlockMissions(Scene scene, LoadSceneMode mode)
    {
        if (MissionsUnlocked || scene.name != "MainScene")
            return;

        MissionsUnlocked = true;
        foreach (var item in MissionsToStart)
        {
            MissionManager.instance.StartMission(item);
        }
    }

    private void UnlockPlaces(Scene scene, LoadSceneMode mode)
    {
        if (placesUnlocked || scene.name != "MainScene")
            return;

        placesUnlocked = true;
    }
}

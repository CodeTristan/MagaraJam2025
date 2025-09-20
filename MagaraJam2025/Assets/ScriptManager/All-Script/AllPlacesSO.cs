using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/AllPlaces")]
public class AllPlacesSO : ScriptableObject
{
    public List<PlaceSO> places;

    private Dictionary<string, PlaceName> placeNameDict;
    private Dictionary<PlaceName, Place> placeDict;


    public PlaceName GetPlaceNameEnum(string name)
    {
        if (placeNameDict.TryGetValue(name, out var outValue))
            return outValue;


        Debug.LogError("Place not found!! --> " + name);
        return PlaceName.Null;
    }

    public Place GetPlaceByName(PlaceName placeName)
    {
        if (placeName != PlaceName.Null && placeDict.TryGetValue(placeName, out var place))
        {
            return place;
        }

        Debug.LogError("Place error! No place in MapManager called --> " + placeName);
        return null;
    }

    public List<Place> GetAllPlaces()
    {
        return placeDict.Values.ToList();
    }

    public void Init()
    {
        if (places == null) places = new List<PlaceSO>();

        placeNameDict = places.ToDictionary(place => place.placeName.GetDisplay(), place => place.placeName);

        placeDict = places.ToDictionary(place => place.placeName, place => new Place(place));

        Debug.Log("AllPlaces initialized with " + places.Count + " places.");

    }

    public void Init(PlaceSaveData[] saveData)
    {
        Init();
        foreach (var data in saveData)
        {
            if (placeDict.TryGetValue(data.placeName, out var place))
            {
                place.HasBeenVisited = data.HasBeenVisited;
                place.isLocked = data.isLocked;
                place.dialogTriggerStates = data.dialogTriggerStates.Select(trigger => new DialogTriggerState(trigger)).ToList();
            }
            else
            {
                Debug.LogWarning("Place not found in dictionary: " + data.placeName);
            }
        }
    }

    public PlaceSaveData[] GetSaveData()
    {
        return placeDict.Values.Select(place => new PlaceSaveData(place)).ToArray();
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Update AllPlacesSO")]
    public static void UpdateAllBackgroundDialogs()
    {
        // AllBackgroundDialogsSO'yu bul
        string allDialogsPath = "Assets/ScriptableObjects/Places/ALLPlaces.asset";
        var allDialogsSO = AssetDatabase.LoadAssetAtPath<AllPlacesSO>(allDialogsPath);

        if (allDialogsSO == null)
        {
            Debug.LogError("AllBackgroundDialogsSO bulunamadý! Yol doðru mu?");
            return;
        }

        // BackgroundDialogSO'larý klasörden yükle
        string[] guids = AssetDatabase.FindAssets("t:PlaceSO", new[] { "Assets/ScriptableObjects/Places" });

        allDialogsSO.places = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<PlaceSO>(path))
            .Where(dialog => dialog != null)
            .ToList();

        // Asset'i kirli (dirty) olarak iþaretle ve kaydet
        EditorUtility.SetDirty(allDialogsSO);
        AssetDatabase.SaveAssets();

        Debug.Log($"Toplam {allDialogsSO.places.Count} placeSO yüklendi ve kaydedildi.");
    }
#endif
}
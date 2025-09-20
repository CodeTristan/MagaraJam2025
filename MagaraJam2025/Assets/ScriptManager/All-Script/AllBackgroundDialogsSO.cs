using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/AllBackgroundDialogs")]
public class AllBackgroundDialogsSO : ScriptableObject
{
    public List<BackgroundDialogSO> backgroundDialogs;

    private Dictionary<(BackgroundName, BackgroundCharacterName), BackgroundDialog> backgroundDialogDict;

    public BackgroundDialog GetBackgroundDialogByName(BackgroundName backgroundName, BackgroundCharacterName characterName)
    {
        if (backgroundDialogDict.TryGetValue((backgroundName, characterName), out BackgroundDialog dialog))
            return dialog;


        Debug.LogError($"Background dialog not found for {backgroundName} and {characterName}");
        return null;
    }

    public List<BackgroundDialog> GetAllBackgroundDialogs()
    {
        return backgroundDialogDict.Values.ToList();
    }

    public void Init()
    {
        backgroundDialogDict = backgroundDialogs.ToDictionary(x => (x.backgroundName, x.characterName), x => new BackgroundDialog(x));

        Debug.Log($"All Background Dialogs Initialized: {backgroundDialogDict.Count} dialogs loaded.");
    }

    public void Init(BackgroundDialogSaveData[] saves)
    {
        if(backgroundDialogDict == null)
            backgroundDialogDict = backgroundDialogs.ToDictionary(x => (x.backgroundName, x.characterName), x => new BackgroundDialog(x));

        foreach (var save in saves)
        {
            if (backgroundDialogDict.TryGetValue((save.backgroundName, save.characterName), out BackgroundDialog dialog))
            {
                for(int i = 0; i < save.dialogTriggerStates.Count; i++)
                {
                    dialog.dialogTriggerStates[i].isTriggered = save.dialogTriggerStates[i].isTriggered;
                    dialog.dialogTriggerStates[i].randomDialog = save.dialogTriggerStates[i].randomDialog;
                    dialog.dialogTriggerStates[i].RandomDialogSelected = save.dialogTriggerStates[i].RandomDialogSelected;
                }
            }
            else
            {
                Debug.LogError($"Background dialog not found for {save.backgroundName} and {save.characterName}");
            }
        }
        Debug.Log($"All Background Dialogs Initialized with saves: {backgroundDialogDict.Count} dialogs loaded.");
    }

    public BackgroundDialogSaveData[] GetSaveData()
    {
        return backgroundDialogDict.Values.Select(bg => new BackgroundDialogSaveData(bg)).ToArray();
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Update AllBackgroundDialogsSO")]
    public static void UpdateAllBackgroundDialogs()
    {
        // AllBackgroundDialogsSO'yu bul
        string allDialogsPath = "Assets/ScriptableObjects/BackgroundDialogs/ALLBackgroundDialogs.asset";
        var allDialogsSO = AssetDatabase.LoadAssetAtPath<AllBackgroundDialogsSO>(allDialogsPath);

        if (allDialogsSO == null)
        {
            Debug.LogError("AllBackgroundDialogsSO bulunamadý! Yol doðru mu?");
            return;
        }

        // BackgroundDialogSO'larý klasörden yükle
        string[] guids = AssetDatabase.FindAssets("t:BackgroundDialogSO", new[] { "Assets/ScriptableObjects/BackgroundDialogs" });

        allDialogsSO.backgroundDialogs = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<BackgroundDialogSO>(path))
            .Where(dialog => dialog != null)
            .ToList();

        // Asset'i kirli (dirty) olarak iþaretle ve kaydet
        EditorUtility.SetDirty(allDialogsSO);
        AssetDatabase.SaveAssets();

        Debug.Log($"Toplam {allDialogsSO.backgroundDialogs.Count} BackgroundDialogSO yüklendi ve kaydedildi.");
    }
#endif
}

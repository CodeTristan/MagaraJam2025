using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    public static event BackgroundChangeDelegate OnBackgroundChange;
    public delegate void BackgroundChangeDelegate(Background background);


    [Header("Variables")]
    public bool MapEnabled = true;

    [Header("UI")]
    [SerializeField] private Canvas generalCanvas;
    [SerializeField] private Canvas backgroundCanvas;
    [SerializeField] private Animator generalCanvasAnimator;

    public TextMeshProUGUI Action_Point_Text;

    [Header("Background Sprites")]
    public Background currentBackground;
    [SerializeField] private Background[] backgrounds;

    public Place currentPlace;
    public List<SoundEffect> soundEffects;
    private bool isChangingBG;
    private bool startNextInstruction;
    private Dictionary<string, BackgroundName> backgroundNameDict;

    public void Init()
    {
        instance = this;
        soundEffects = new List<SoundEffect>();
        //currentPlace = GameManager.instance.allPlaces.GetPlaceByName(PlaceName.Home);
        //currentBackground = GetBackgroundByName(BackgroundName.Your_House);
        ToggleGeneralCanvas(true);
        CloseAllBackgrounds();

        backgroundNameDict = new Dictionary<string, BackgroundName>();

        foreach (BackgroundName e in Enum.GetValues(typeof(BackgroundName)))
        {
            string backgroundName = e.GetDisplay();
            backgroundNameDict[backgroundName] = e;
        }

        SetBackgroundDialogs();

    }

    public void SetBackgroundDialogs()
    {
        foreach (Background bg in backgrounds)
        {
            bg.dialogs = new BackgroundDialog[bg.characterButtons.Length];
            for (int i = 0; i < bg.dialogs.Length; i++)
            {
                BackgroundDialog data = GameManager.instance.allBackgroundDialogs.GetBackgroundDialogByName(bg.name, bg.characterButtons[i].name);
                if(data != null)
                {
                    bg.dialogs[i] = data;
                    bg.characterButtons[i].conditions = bg.dialogs[i].data.conditions;
                }
                else
                {
                    Debug.LogWarning("Background Dialog not found for " + bg.name + " and " + bg.characterButtons[i].name);
                }
            }
        }
    }
    public void ToggleBackgroundCanvas(bool toggle)
    {
        backgroundCanvas.enabled = toggle;
    }

    public void EnableGeneralCanvasToggle()
    {
        MapEnabled = true;
    }
    public void DisableGeneralCanvasToggle()
    {
        MapEnabled = false;
    }
    public void ToggleGeneralCanvas(bool enabled)
    {
        //if (VideoManager.instance.inVideo == true)
        //    return;
        if(SahneManager.instance.currentSceneName != "MainScene")
        {
            enabled = false;
        }
        enabled = enabled && MapEnabled; //If map is disabled, general canvas should be disabled too

        generalCanvas.enabled = enabled;
    }
    public void TintBackground(Color32 color)
    {
        //currentBackground.backgroundImage.color = color;
    }
    public void ChangeBackground(string name)
    {
        name = name.Trim();
        //For fade away
        if (name == "FADEAWAY")
        {
            DarkenScreen(1.5f,true);
            return;
        }
        else if(name == "RESETSPRITEUI")
        {
            SpriteManager.instance.ResetAllUI();
            DialogManager.instance.StartNextInstruction();
            return;
        } 
        else if(name == "DISABLEMAP")
        {
            DisableGeneralCanvasToggle();
            ToggleGeneralCanvas(false);
            Debug.Log("Map Disabled");
            DialogManager.instance.StartNextInstruction();
            return;
        }
        else if(name == "ENABLEMAP")
        {
            EnableGeneralCanvasToggle();
            ToggleGeneralCanvas(true);
            Debug.Log("Map Enabled");
            DialogManager.instance.StartNextInstruction();
            return;
        }
        else if (name == "FADEOUT")
        {
            StartCoroutine(Fadeout());
            return;
        }
        else if (name == "FADEIN")
        {
            StartCoroutine(FadeIn());
            return;
        }
        else if (name == "NONE")
        {
            CloseAllBackgrounds();
            DialogManager.instance.StartNextInstruction();
            return;
        }


        Background bg = GetBackgroundByName(GetBackgroundNameEnum(name));
        if (bg == null)
        {
            Debug.LogError("Returned in ChangeBackground, because background is null!! --> " + name);
            return;
        }

        if(bg != currentBackground)
            CheckForSounds(bg);
        CloseAllBackgrounds();
        currentBackground = bg;
        bg.backgroundObject.SetActive(true);
        bg.CheckCharacterConditions();

        //bg.CheckCharacterConditions();
        //CheckBackgroundCharacters(bg);
        //currentPlace = MapManager.instance.GetPlaceByName(name);
        DialogManager.instance.StartNextInstruction();
    }
    public void ChangeBackground(Place place,bool WithAnim,bool checkDialogs = true,bool startNextInstruction = false)
    {
        this.startNextInstruction = startNextInstruction;
        if(place == null) return;
        Background bg = GetBackgroundByName(place.PlaceData.backgroundName);
        currentPlace = place;
        if (bg == null)
        {
            Debug.LogError("Returned in ChangeBackground, because background is null!! --> " + place.PlaceData.placeName);
            return;
        }
        if (isChangingBG)
            return;

        if(bg != currentBackground)
            CheckForSounds(bg);
        MusicManager.instance.PlayMusic(place.PlaceData.MusicName, true);
        if(WithAnim)
        {
            isChangingBG = true;
            StartCoroutine(StartChangePlaceAnim(checkDialogs));
        }
        else
        {
            ChangePlaceAnimation();
            ChangePlaceAnimationEnd(checkDialogs);
        }
        
    }

    private void CheckBackgroundCharacters(Background bg)
    {
        for (int i = 0; i < bg.characterButtons.Length; i++)
        {
            Background.BGCharacter character = bg.characterButtons[i];
            if (character == null && character.characterButton == null)
                continue;

            for (int j = 0; j < bg.dialogs.Length; j++)
            {
                if (bg.dialogs[j].data.characterName != character.name || character.HasDialogs == false)
                    continue;

                character.characterButton.onClick.RemoveAllListeners();
                BackgroundDialog dialog = bg.dialogs[j];
                character.characterButton.onClick.AddListener(() => dialog.CheckDialogToTrigger());
                //Debug.Log("Listener added : " + character.name + "  " + dialog.dialogTriggers.Count);
                break;
            }
        }
        bg.CheckCharacterConditions();
    }
    private IEnumerator StartChangePlaceAnim(bool checkDialogs)
    {
        generalCanvasAnimator.SetTrigger("FadeAway");
        yield return new WaitForSeconds(0.5f);
        ChangePlaceAnimation();
        yield return new WaitForSeconds(0.5f);
        ChangePlaceAnimationEnd(checkDialogs);
    }
    private void CheckForSounds(Background bg)
    {
        foreach (var item in soundEffects.ToArray())
        {
            if(item.source.loop)
            {
                item.kill();
                soundEffects.Remove(item);
            }
        }
        if (bg.soundEffects.Length > 0 && currentBackground != bg)
        {
            int r = UnityEngine.Random.Range(0, bg.soundEffects.Length);
            //Debug.Log(r + " / " + bg.soundEffects.Length);
            MusicManager.instance.PlaySound(bg.soundEffects[r]);
        }

        foreach (var item in bg.loopingSounds)
        {
            MusicManager.instance.PlaySound(item);
            soundEffects.Add(MusicManager.instance.SoundEffects[MusicManager.instance.SoundEffects.Count - 1]);
        }
    }
    public void ToggleDarkness(bool darkness)
    {
        if (darkness)
        {
            generalCanvasAnimator.SetTrigger("Darken");
        }
        else
        {
            generalCanvasAnimator.SetTrigger("Lighten");
        }
    }
    public void DarkenScreen(float DarkTime,bool startNextInstruction = false)
    {
        StartCoroutine(DarkenScreenNumerator(DarkTime,startNextInstruction));
    }
    public IEnumerator DarkenScreenNumerator(float timer, bool startNextInstruction)
    {
        generalCanvasAnimator.SetTrigger("Darken");

        yield return new WaitForSeconds(timer);

        generalCanvasAnimator.SetTrigger("Lighten");
        yield return new WaitForSeconds(0.2f);

        if(startNextInstruction)
        {
            DialogManager.instance.StartNextInstruction();
        }
    }

    public IEnumerator Fadeout()
    {
        generalCanvasAnimator.SetTrigger("Darken");

        yield return new WaitForSeconds(generalCanvasAnimator.GetCurrentAnimatorStateInfo(0).length);

        DialogManager.instance.StartNextInstruction();
    }

    public IEnumerator FadeIn()
    {
        generalCanvasAnimator.SetTrigger("Lighten");

        yield return new WaitForSeconds(.4f);

        DialogManager.instance.StartNextInstruction();
    }
    public void ChangePlaceAnimation()
    {
        Background bg = GetBackgroundByName(currentPlace.PlaceData.backgroundName);
        CloseAllBackgrounds();
        bg.backgroundObject.SetActive(true);
        CheckBackgroundCharacters(bg);

    }

    public void ChangePlaceAnimationEnd(bool checkDialogs)
    {
        Background bg = GetBackgroundByName(currentPlace.PlaceData.backgroundName);
        currentBackground = bg;
        isChangingBG = false;
        ToggleGeneralCanvas(true);
        if (startNextInstruction)
            DialogManager.instance.StartNextInstruction();
        if(checkDialogs)
            currentPlace.CheckDialogToTrigger();

        OnBackgroundChange?.Invoke(bg);

    }
    public Background GetBackgroundByName(BackgroundName name)
    {
        foreach (Background bg in backgrounds)
        {
            if (bg.name == name)
            {
                return bg;
            }
        }

        Debug.LogError("Background is not found in BackgroundManager! --> '" + name +"'");
        return null;
    }

    public BackgroundName GetBackgroundNameEnum(string name)
    {
        BackgroundName outValue;

        if (backgroundNameDict.TryGetValue(name, out outValue))
            return outValue;
        else
        {
            Debug.LogError("Background not found!! --> " + name);
            return BackgroundName.Null;
        }
    }

    public void CloseAllBackgrounds()
    {
        for(int i = 1; i < backgrounds.Length; i++)
        {
            backgrounds[i].backgroundObject.SetActive(false);
        }
    }

    public void FindCamera()
    {
        backgroundCanvas.worldCamera = Camera.main;
    }
}

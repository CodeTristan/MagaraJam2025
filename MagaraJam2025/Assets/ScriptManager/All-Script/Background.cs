using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Background
{
    [SerializeField] private string TitleName; //For unity inspector

    public BackgroundName name;
    public GameObject backgroundObject;
    public Image backgroundImage;
    public Sound[] soundEffects;
    public Sound[] loopingSounds;
    public BGCharacter[] characterButtons;
    public ClickableImage[] clickableImages;
    public BackgroundDialog[] dialogs;

    public delegate void BackgroundButtonAction();

    [System.Serializable]
    public class BGCharacter
    {
        public BackgroundCharacterName name;
        public GameObject characterGameObject;
        public Button characterButton;
        [HideInInspector] public List<CheckCondition> conditions;
        public List<BackgroundButtonAction> buttonActions;
        public bool HasDialogs = true;

        public BGCharacter()
        {
            conditions = new List<CheckCondition>();
            buttonActions = new List<BackgroundButtonAction>();
        }
    }

    public void CheckCharacterConditions()
    {
        foreach (var character in characterButtons)
        {
            List<Condition> conditions = new List<Condition>();
            conditions.AddRange(character.conditions);
            if (ConditionManager.instance.CheckConditions(conditions))
            {
                character.characterButton.gameObject.SetActive(true);
                character.characterGameObject.gameObject.SetActive(true);
            }
            else
            {
                character.characterButton.gameObject.SetActive(false);
                character.characterGameObject.gameObject.SetActive(false);
            }
        }

        foreach (var cImage in clickableImages)
        {
            cImage.Init();
        }
    }

    public void AddButtonAction(BackgroundCharacterName characterName, BackgroundButtonAction action)
    {
        foreach (var character in characterButtons)
        {
            if (character.name == characterName)
            {
                character.buttonActions.Add(action);
            }
        }
    }
    public void AssignButtonActions()
    {
        foreach (var character in characterButtons)
        {
            character.characterButton.onClick.RemoveAllListeners();
            foreach (var action in character.buttonActions)
            {
                character.characterButton.onClick.AddListener(() => { action(); });
            }
        }
    }
}

public enum BackgroundName
{
    [DisplayName("Null")] Null,

}

public enum BackgroundCharacterName
{

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpriteUI
{
    public string CharacterName;
    public Image image;
    public Animator animator;
}

[System.Serializable]
public class CharacterSprite
{
    public string CharacterName;
    public Color32 CharacterColor;
    public Sprite[] Sprites;

    [System.Serializable]
    public class Sprite
    {
        public string SpriteName;
        public UnityEngine.Sprite sprite;
        public Sound[] soundEffects;
    }
}

[System.Serializable]
public class ItemSprite
{
    public string ItemName;
    public Sprite sprite;
}
public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    [Header("UI")]
    [SerializeField] private SpriteUI[] spriteUIs;

    [Header("Character Sprites")]
    [SerializeField] CharacterSprite[] CharacterSprites;

    [Header("Item Sprites")]
    [SerializeField] ItemSprite[] ItemSprites;

    private Queue<string> speakers;
    bool animatorIsChanging;
    

    public void Init()
    {
        instance = this;
        speakers = new Queue<string>();
        ResetAllUI();
    }

    public ItemSprite GetItemSprite(string itemName)
    {
        foreach (var item in ItemSprites)
        {
            if(item.ItemName == itemName)
                return item;
        }

        Debug.LogError("ItemSprite not found!! --> " + itemName);
        return null;
    }

    public void ResetAllUI()
    {
        foreach(SpriteUI sprite in spriteUIs)
        {
            sprite.CharacterName = "";
            sprite.image.sprite = null;
            sprite.image.gameObject.SetActive(false);
        }
        speakers.Clear();
    }

    public bool ChangeSprite(string CharacterName, string SpriteName)
    {
        CharacterSprite character = FindCharacterSpriteByName(CharacterName);
        if (character == null)
            return false;
        CharacterSprite.Sprite sprite = FindSpriteByName(character, SpriteName);
        if (sprite == null) 
            return false;

        if(sprite.soundEffects.Length > 0)
        {
            int r = Random.Range(0, sprite.soundEffects.Length);
            MusicManager.instance.PlaySound(sprite.soundEffects[r]);
        }
        if(IsCharacterInQueue(CharacterName))
        {
            animatorIsChanging = false;
        }
        else
        {
            animatorIsChanging = true;
            SpriteUI spriteUI;
            if(speakers.Count >= spriteUIs.Length)
            {
                spriteUI = FindSpriteUIByName(speakers.Dequeue());
            }
            else
            {
                spriteUI = spriteUIs[speakers.Count];
            }

            speakers.Enqueue(CharacterName);
            
            spriteUI.CharacterName = CharacterName;
            spriteUI.image.gameObject.SetActive(true);
        }
        AdjustUI(CharacterName, sprite.sprite);

        return true;
    }

    public void AdjustUI(string CharacterName,Sprite sprite)
    {
        foreach (SpriteUI item in spriteUIs)
        {
            item.image.color = new Color32(170,170,170, 255);
            bool isTalking = false;
            if(item.CharacterName == CharacterName)
            {
                item.image.sprite = sprite;
                isTalking = true;
                item.image.rectTransform.SetSiblingIndex(3);
                item.image.color = new Color32(255, 255, 255, 255);
            }

            //item.animator.SetBool("IsTalking", isTalking);
            //item.animator.SetBool("IsChanging",animatorIsChanging);
        }
    }

    private SpriteUI FindSpriteUIByName(string CharacterName)
    {
        foreach(SpriteUI item in spriteUIs)
        {
            if (item.CharacterName == CharacterName)
                return item;
        }

        return null;
    }
    private bool IsCharacterInQueue(string CharacterName)
    {
        foreach (string item in speakers)
        {
            if(item == CharacterName)
                return true;
        }

        return false;
    }

    public CharacterSprite FindCharacterSpriteByName(string CharacterName)
    {
        foreach (CharacterSprite sprite in CharacterSprites)
        {
            if(sprite.CharacterName == CharacterName)
                return sprite;
        }

        Debug.LogError("CharacterSprite returned null in SpriteManager:FindCharacterSpriteByName!! --> " + CharacterName);
        return null;
    }

    private CharacterSprite.Sprite FindSpriteByName(CharacterSprite character,string SpriteName)
    {
        if(character.Sprites.Length == 0)
        {
            Debug.LogError("CharacterSprite has no sprites in SpriteManager:FindSpriteByName!! --> " + character.CharacterName + "-" + SpriteName);
            return null;
        }

        if (character.Sprites[0].SpriteName.ToUpper() == "NOSPRITE")
            return null; //No error message because we dont want any sprites

        if (SpriteName.ToUpper() == "NONE")
            return character.Sprites[0];



        foreach (CharacterSprite.Sprite sprite in character.Sprites)
        {
            if (SpriteName == sprite.SpriteName)
                return sprite;
        }


        Debug.LogError("Sprite returned null in SpriteManager:FindSpriteByName!! --> " + character.CharacterName + "-" + SpriteName);
        return null;
    }
}

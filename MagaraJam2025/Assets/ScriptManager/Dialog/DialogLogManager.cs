using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogLogManager : MonoBehaviour
{
    public static DialogLogManager instance;
    public int MaxLogCount;

    [Header("UI")]
    [SerializeField] private Canvas LogCanvas;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI LogText;


    public List<Instruction> LogInstructions;
    public List<Dialog> LogDialogs;

    private ControlType[] controlTypes;

    public void Init()
    {
        instance = this;
        LogInstructions = new List<Instruction>();
        LogDialogs = new List<Dialog>();
        LogCanvas.enabled = false;
        InputManager.instance.EnableLogControls();
    }

    public void OpenDialogLog(InputAction.CallbackContext context)
    {
        if (LogCanvas.enabled == false)
        {
            _OpenDialogBox();
        }
        else
        {
            CloseDialogLog(context);
        }
    }

    public void _ToggleDialogBox()
    {
        if (LogCanvas.enabled == false)
        {
            _OpenDialogBox();
        }
        else
        {
            CloseDialogLog();
        }
    }
    public void _OpenDialogBox()
    {
        DialogManager.instance.inLog = true;
        LogCanvas.enabled = true;
        AdjustUI();
        controlTypes = InputManager.instance.ActiveControls.ToArray();
        InputManager.instance.DisableAllControls();
        scrollRect.verticalNormalizedPosition = 0f;
    }
    public void CloseDialogLog(InputAction.CallbackContext context)
    {
        if(LogCanvas.enabled)
            CloseDialogLog();
    }

    public void CloseDialogLog()
    {
        DialogManager.instance.inLog = false;
        LogCanvas.enabled = false;
        InputManager.instance.EnableControls(controlTypes);
    }

    private void AdjustUI()
    {
        LogText.text = "\n";
        foreach (Instruction i in LogInstructions)
        {
            switch (i.type)
            {
                case InstructionType.Dialog:
                    {
                        Dialog dialog = (Dialog)i;
                        if (dialog.name.ToUpper() == "NONE")
                            dialog.name = "";
                        if(dialog.name == "MC")
                            dialog.name = DialogManager.instance.PlayerName;
                        CharacterSprite characterSprite = SpriteManager.instance.FindCharacterSpriteByName(dialog.name);
                        Color32 color = characterSprite != null ? characterSprite.CharacterColor : new Color32(255,255,255,255);
                        LogText.text += "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + dialog.name + "</color>\n";
                        foreach (var s in dialog.sentences)
                        {
                            string sentence = s;
                            //Renaming MC to Player Name
                            while (sentence.IndexOf("#MC") != -1)
                            {
                                string after = sentence.Substring(sentence.IndexOf("#MC") + 3);
                                string before = sentence.Substring(0, sentence.IndexOf("#MC"));
                                before += DialogManager.instance.PlayerName;
                                sentence = before + after;
                            }
                            if(dialog.name == "")
                                LogText.text += sentence + "\n";
                            else
                                LogText.text += "> " + sentence + "\n";
                        }

                        LogText.text += "\n";
                        break;
                    }
                case InstructionType.ChoiceBody:
                    {
                        ChoiceBody choiceBody = (ChoiceBody)i;
                        LogText.text += "CHOICE\n";
                        for(int j = 0; j < choiceBody.choices.Count; j++)
                        {
                            string colorText;
                            if (choiceBody.LastSelectedIndex == j)
                                colorText = "<color=green>";
                            else
                                colorText = "<color=white>";
                            LogText.text += colorText + (j + 1) + "- " + choiceBody.choices[j].choiceText + "</color>\n";
                        }
                        LogText.text += "\n";
                        break;
                    }
                default:
                    break;
            }

            LogText.rectTransform.sizeDelta = new Vector2(LogText.rectTransform.sizeDelta.x, LogText.preferredHeight);
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
    public void AddLog(Instruction ins)
    {
        if (LogInstructions.Count > MaxLogCount)
        {
            LogInstructions.RemoveAt(0);
        }

        if(ins.type == InstructionType.ChoiceBody)
        {
            ChoiceBody choiceBody = (ChoiceBody)ins;
            ChoiceBody cb = new ChoiceBody(choiceBody.choices);
            cb.LastSelectedIndex = choiceBody.LastSelectedIndex;
            LogInstructions.Add(cb);
            return;
        }
        else if(ins.type == InstructionType.Dialog)
        {
            Dialog dialog = (Dialog)ins;
            Dialog d = new Dialog();
            d.name = dialog.name;
            LogInstructions.Add(d);
            LogDialogs.Add(d);
        }

    }
}

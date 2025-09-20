using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Xml.Serialization;
using System.Xml;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;

    public ChoiceBody currentChoiceBody;
    public delegate void ChoiceFunction(int choiceIndex);

    [Header("Color")]
    [SerializeField] private Color32 DefaultChoiceColor;
    [SerializeField] private Color32 SelectedChoiceColor;
    [SerializeField] private Color32 DefaultTextColor;
    [SerializeField] private Color32 SelectedTextColor;

    [Header("UI")]
    [SerializeField] private Canvas ChoiceCanvas;
    [SerializeField] private RectTransform ScrollRectContent;
    [SerializeField] private RectTransform bottomRectTransform;
    [SerializeField] private GameObject ChoiceGameObjectPrefab;
    [SerializeField] private TextMeshProUGUI ChoiceButtonInnerText;
    [SerializeField] private GameObject choiceGoNextButton;
    [SerializeField] private GameObject choiceGoPreviousButton;
    public int SelectedIndex;
    public bool inChoice;

    private List<RectTransform> ChoiceButtons;
    private int ChoiceUI_Index = 0;
    List<List<RectTransform>> ChoiceUIObjects;
    private List<ChoiceFunction> currentEventChoiceFunctions;
    private bool breakLoop;
    public void Init()
    {
        instance = this;
        ChoiceButtons = new List<RectTransform>();
        ChoiceCanvas.enabled = false;
    }

    public void StartChoiceWithEvent(ChoiceBody choiceBody,List<ChoiceFunction> functions)
    {
        DialogManager.instance.CloseNextDialogButton();
        InputManager.instance.DisableAllControls();
        ChoiceCanvas.enabled = true;
        DeleteAllChoiceButtons();
        currentChoiceBody = choiceBody;
        currentEventChoiceFunctions = functions;
        inChoice = true;
        breakLoop = false;


        int j = 0;
        foreach (Choice choice in choiceBody.choices)
        {
            if (choice.isVisible == false)
                continue;

            ChoiceButtonInnerText.text = (j+1) + "- " + choice.choiceText;
            RectTransform obj = Instantiate(ChoiceGameObjectPrefab, ScrollRectContent.transform).GetComponent<RectTransform>();
            Button button = obj.GetComponentInChildren<Button>();
            ChoiceFunction func = functions[j];
            int index = j;
            button.onClick.AddListener(() => func(index));
            button.interactable = choice.isSelectable;
            obj.gameObject.SetActive(true);
            ChoiceButtons.Add(obj);
            j++;
        }

        if (choiceBody.DialogText != "")
        {
            DialogManager.instance.nameText.text = "";
            DialogManager.instance.dialogText.text = choiceBody.DialogText;
            DialogManager.instance.dialogText.maxVisibleCharacters = choiceBody.DialogText.Length;
            DialogManager.instance.OpenDialogCanvas(false);
        }

        AdjustUI();
        InputManager.instance.EnableEventChoiceControls();
    }

    public void StartChoice(ChoiceBody choiceBody = null)
    {
        DialogManager.instance.CloseNextDialogButton();
        if (choiceBody != null)
        {
            currentChoiceBody = choiceBody;
        }

        ChoiceCanvas.enabled = true;
        inChoice = true;
        breakLoop = false;
        DeleteAllChoiceButtons();

        int i = 1;
        foreach (Choice choice in choiceBody.choices)
        {
            if (choice.isVisible == false)
                continue;


            ChoiceButtonInnerText.text = (i++) + "- " + RedesignSentence(choice.choiceText);
            GameObject obj = Instantiate(ChoiceGameObjectPrefab, ScrollRectContent);
            Button button = obj.GetComponentInChildren<Button>();
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            button.onClick.AddListener(() => _ConfirmChoice(choice));

            if(choice.hasSelected)
            {
                button.image.color = SelectedChoiceColor;
                text.color = SelectedTextColor;
            }
            else
            {
                button.image.color = DefaultChoiceColor;
                text.color = DefaultTextColor;
            }

            RectTransform objRectTransform = obj.GetComponent<RectTransform>();
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

            objRectTransform.sizeDelta = new Vector2(900, 150 + (50 * (choice.choiceText.Length / 100)));
            buttonRectTransform.sizeDelta = new Vector2(900, 100 + (50 * (choice.choiceText.Length / 100)));

            obj.SetActive(true);
            ChoiceButtons.Add(objRectTransform);
        }

        if (currentChoiceBody.isLoop)
        {
            DialogManager.instance.currentManagerVariables.beforeLoopChoiceIndex = DialogManager.instance.currentManagerVariables.instructionIndex;
            if (DialogManager.instance.currentManagerVariables.beforeLoopChoiceIndex < 0)
                DialogManager.instance.currentManagerVariables.beforeLoopChoiceIndex = 0;
            else if (DialogManager.instance.currentManagerVariables.instructions.Count <= DialogManager.instance.currentManagerVariables.beforeLoopChoiceIndex)
                DialogManager.instance.currentManagerVariables.beforeLoopChoiceIndex = DialogManager.instance.currentManagerVariables.instructions.Count-1;
            DialogManager.instance.currentManagerVariables.isLooping = true;
        }
        if(currentChoiceBody.DialogText != "")
        {
            DialogManager.instance.nameText.text = "";
            DialogManager.instance.dialogText.text = currentChoiceBody.DialogText;
            DialogManager.instance.dialogText.maxVisibleCharacters = currentChoiceBody.DialogText.Length;
        }
        if(DialogManager.instance.dialogText.text != "")
            DialogManager.instance.OpenDialogCanvas(false);
        AdjustUI();
    }

    private void AdjustUI()
    {
        ChoiceUIObjects = new List<List<RectTransform>>();
        float y = 100;
        int size = 0;
        int i = 1;
        List<RectTransform> list = new List<RectTransform>();
        foreach (RectTransform item in ChoiceButtons)
        {
            y += item.sizeDelta.y;
            TextMeshProUGUI text = item.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            string newText = text.text;
            newText = newText.Substring(3, newText.Length - 3);
            text.text = newText.Insert(0, i + ") ");
            i++;
            if(y >= bottomRectTransform.sizeDelta.y)
            {
                y = 100;
                ChoiceUIObjects.Add(list);
                list = new List<RectTransform>();
                size++;
            }
            list.Add(item);
        }
        ChoiceUIObjects.Add(list);

        choiceGoNextButton.SetActive(false);
        choiceGoPreviousButton.SetActive(false);
        if (size != 0)
        {
            ChoiceUI_Index = 0;
            _GoNextChoice();
        }

    }

    public void _GoNextChoice()
    {
        foreach (List<RectTransform> list in ChoiceUIObjects)
        {
            foreach (RectTransform item in list)
            {
                item.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < ChoiceUIObjects.Count; i++)
        {
            if(ChoiceUI_Index == i)
            {
                foreach (RectTransform item in ChoiceUIObjects[i])
                {
                    item.gameObject.SetActive(true);
                }
            }
        }

        ChoiceUI_Index++;
        if (ChoiceUI_Index == ChoiceUIObjects.Count)
        {
            choiceGoNextButton.SetActive(false);
            choiceGoPreviousButton.SetActive(true);
            ChoiceUI_Index--;
        }
        else
        {
            choiceGoNextButton.SetActive(true);
            
        }
        choiceGoNextButton.transform.SetAsLastSibling();

    }

    public void _GoPreviousChoice()
    {
        ChoiceUI_Index--;

        foreach (List<RectTransform> list in ChoiceUIObjects)
        {
            foreach (RectTransform item in list)
            {
                item.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < ChoiceUIObjects.Count; i++)
        {
            if (ChoiceUI_Index == i)
            {
                foreach (RectTransform item in ChoiceUIObjects[i])
                {
                    item.gameObject.SetActive(true);
                }
            }
        }

        if (ChoiceUI_Index == 0)
        {
            choiceGoPreviousButton.SetActive(false);
            choiceGoNextButton.SetActive(true);
            
            ChoiceUI_Index++;
        }
        else
        {
            choiceGoPreviousButton.SetActive(true);
        }
        choiceGoNextButton.transform.SetAsLastSibling();
    }

    public void _ConfirmChoice(Choice choice)
    {
        if (!choice.isSelectable)
            return;

        if(choice.choiceText == "(Leave)")
        {
            breakLoop = true;
        }
        foreach (int index in choice.choicesToActivate)
        {
            currentChoiceBody.choices[index].isVisible = true;
        }
        int choiceIndex = 0;
        for(int i = 0; i < currentChoiceBody.choices.Count; i++)
        {
            if (choice == currentChoiceBody.choices[i])
                choiceIndex = i;
        }
        currentChoiceBody.LastSelectedIndex = choiceIndex;
        DialogLogManager.instance.AddLog(currentChoiceBody);

        choice.hasSelected = true;
        inChoice = false;
        ChoiceCanvas.enabled = false;
        DialogManager.instance.CloseDialogCanvas(true);
        InputManager.instance.DisableChoiceControls();
        DialogManager.instance.StartBranch(choice.branch);
    }

    public void ConfirmChoiceEvent(int choiceIndex)
    {
        ChoiceCanvas.enabled = false;
        inChoice = false;
        DialogManager.instance.inDialog = false;
        DialogManager.instance.CloseDialogCanvas(true);
        DialogManager.instance.ResetDialogManager();
        BackgroundManager.instance.ToggleGeneralCanvas(true);

        InputManager.instance.DisableEventChoiceControls();
        InputManager.instance.EnableMainSceneControl();
    }
    public bool IsLoopFinished(ChoiceBody choiceBody)
    {
        if (breakLoop) return true;

        foreach (Choice item in choiceBody.choices)
        {
            if (item.hasSelected == false)
                return false;
        }


        return true;
    }

    public void InputConfirmChoice(InputAction.CallbackContext context)
    {
        string keyPressed = context.control.name;
        if(keyPressed.Length > 1) //If numpad is detected
        {
            keyPressed = keyPressed[keyPressed.Length - 1].ToString();
        }
        try
        {
            _ConfirmChoice(currentChoiceBody.choices[int.Parse(keyPressed)-1]);
        }
        catch
        {
            Debug.LogError("InputConfirmChoice error!!!! --> " + keyPressed);
        }
    }
    public void InputEventConfirmChoice(InputAction.CallbackContext context)
    {
        string keyPressed = context.control.name;
        if (keyPressed.Length > 1) //If numpad is detected
        {
            keyPressed = keyPressed[keyPressed.Length - 1].ToString();
        }
        try
        {
            int index = int.Parse(keyPressed) - 1;
            if (currentChoiceBody.choices[index].isSelectable)
                currentEventChoiceFunctions[index].Invoke(index);

        }
        catch
        {
            Debug.LogError("InputEventConfirmChoice error!!!! --> " + keyPressed);
        }
    }

    public void ResetChoiceHasSelected(string branchName,string fileName,string choiceName)
    {
        FileXML file = DialogManager.instance.GetFileByName(fileName);
        DialogBranch branch = DialogManager.instance.GetBranchByName(file, branchName);

        foreach (ChoiceBody cb in branch.choiceBodies)
        {
            if(cb.name == choiceName)
            {
                foreach (Choice item in cb.choices)
                {
                    item.hasSelected = false;
                }
                return;
            }
        }

        Debug.LogError("ChoiceManager:ResetChoiceHasSelected choice not found!! " + branchName + "-" + fileName + "-" + choiceName);
    }

    public void CloseChoiceCanvas()
    {
        ChoiceCanvas.enabled = false;
    }
    private void DeleteAllChoiceButtons()
    {
        foreach (RectTransform item in ChoiceButtons)
        {
            Destroy(item.gameObject);
        }
        ChoiceButtons.Clear();
    }

    private string RedesignSentence(string sentence)
    {
        //Renaming MC to Player Name
        while (sentence.IndexOf("#MC") != -1)
        {
            string after = sentence.Substring(sentence.IndexOf("#MC") + 3);
            string before = sentence.Substring(0, sentence.IndexOf("#MC"));
            before += DialogManager.instance.PlayerName;
            sentence = before + after;
        }

        return sentence;
    }

    //TEMPORARY SOLUTION CHANGE LATER
    private void CheckChoices(string fileName,string branchName,int choiceIndex)
    {
        string tag = fileName + "/" + branchName + "/" + choiceIndex;
        Debug.Log(tag);
        switch (tag)
        {
            case "/Question/1/3":
                {
                    MissionManager.instance.QuestHelpQuestion();
                    break;
                }
        }
    }

    
}

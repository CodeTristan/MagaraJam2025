using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NotebookController : MonoBehaviour
{
    public static NotebookController Instance { get; private set; }

    public bool IsOpen { get; private set; } = false;

    public float TypeDelay = 0.02f;
    public NotebookAnimator notebookAnimator;

    public GameObject NotebookUI;
    public ClueUIObject[] ClueTexts;

    public TextMeshProUGUI ConnectClueText1;
    public TextMeshProUGUI ConnectClueText2;
    public TextMeshProUGUI CreatedClueText;

    public List<Clue> AllClues = new List<Clue>();
    
    private List<Clue> currentClues = new List<Clue>();
    private List<Clue> usedClues = new List<Clue>();

    private Clue selectedClue1 = null;
    private Clue selectedClue2 = null;

    private Dictionary<ClueName, string> ClueNamePairs;
    private int LastClueIndex = 0;
    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        ClueNamePairs = AllClues.ToDictionary(clue => clue.clueName, clue => clue.clueName.GetDisplay());
        Instance = this;
        ClearSelections();
    }

    public bool AddClue(ClueName clueName, bool startNextInstruction)
    {
        Clue clueToAdd = AllClues.Find(clue => clue.clueName == clueName);
        if (clueToAdd != null && !currentClues.Contains(clueToAdd) && !usedClues.Contains(clueToAdd))
        {
            if(!IsOpen)
                notebookAnimator.OnPointerClick(null); // Open the notebook when a new clue is added

            UpdateClueList();
            StartCoroutine(TypeText(ClueTexts[LastClueIndex].Text, clueToAdd.description,startNextInstruction));
            currentClues.Add(clueToAdd);
            return true;
        }
        return false;
    }

    public bool AddClue(string name)
    {
        if (ClueNamePairs.ContainsValue(name))
        {
            ClueName clueName = ClueNamePairs.FirstOrDefault(x => x.Value == name).Key;
            return AddClue(clueName,true);
        }
        else
        {
            Debug.LogError($"Clue with name {name} not found in ClueNamePairs.");
        }
        return false;
    }

    private void SelectClue(ClueUIObject obj)
    {
        if (selectedClue1 == null || selectedClue1.clueName == ClueName.None)
        {
            selectedClue1 = obj.clue;
            ConnectClueText1.text = selectedClue1.description;
        }
        else if (selectedClue2 == null || selectedClue2.clueName == ClueName.None && obj.clue.description != selectedClue1.description)
        {
            selectedClue2 = obj.clue;
            ConnectClueText2.text = selectedClue2.description;
        }
        else
        {
            selectedClue1 = obj.clue;
            ConnectClueText1.text = selectedClue1.description;
            selectedClue2 = null;
            ConnectClueText2.text = "";
        }
    }


    public void ToggleNotebook()
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            UpdateClueList();
        }
        else
        {

        }
    }

    private void UpdateClueList()
    {
        foreach (var text in ClueTexts)
        {
            text.Text.text = "";
        }

        for (int i = 0; i < currentClues.Count && i < ClueTexts.Length; i++)
        {
            ClueTexts[i].clue = currentClues[i];
            ClueTexts[i].Text.text = currentClues[i].description;
            LastClueIndex = i+1;
            ClueTexts[i].Button.onClick.RemoveAllListeners();
            int index = i; // Capture the current index
            ClueTexts[i].Button.onClick.AddListener(() => SelectClue(ClueTexts[index]));
        }
    }

    private IEnumerator TypeText(TextMeshProUGUI text,string sentence, bool startNextInstruction = true)
    {
        yield return new WaitForSeconds(0.2f);
        text.text = "";
        bool isRichTextTag = false;

        //Typing Sentence
        char[] array = sentence.ToCharArray();

        int DialogTextMaxVisibleCharacter = 0;
        int RichTagCharacter = 0;
        text.text = sentence;

        for (int i = 0; i < array.Length; i++)
        {
            DialogTextMaxVisibleCharacter++;
            text.maxVisibleCharacters = DialogTextMaxVisibleCharacter;
            char letter = array[i];
            if (letter == '<' || isRichTextTag)
            {
                isRichTextTag = true;
                //dialogText.text += letter;
                if (letter == '>')
                    isRichTextTag = false;
                RichTagCharacter++;
                DialogTextMaxVisibleCharacter--;

            }
            else
            {
                yield return new WaitForSeconds(TypeDelay);
            }

        }

        UpdateClueList();

        if (startNextInstruction)
            DialogManager.instance.StartNextInstruction();

    }

    private void ClearSelections()
    {
        selectedClue1 = null;
        selectedClue2 = null;
        ConnectClueText1.text = "";
        ConnectClueText2.text = "";
        CreatedClueText.text = "";
    }

    public void _ConnectClues()
    {
        switch (selectedClue1,selectedClue2)
        {
            case (Clue c1, Clue c2) when ((c1.clueName == ClueName.Scar && c2.clueName == ClueName.FacePaint) || (c2.clueName == ClueName.Scar && c1.clueName == ClueName.FacePaint)):
                AddClue(ClueName.Framer_is_Liar,false);
                CreatedClueText.text = "Yeni ipucu oluþturuldu: " + AllClues.Find(x => x.clueName == ClueName.Framer_is_Liar).description;
                break;
            default:
                ClearSelections();
                CreatedClueText.text = "Bu ipuçlarý arasýnda bir baðlantý yok.";
                return;
        }

        currentClues.Remove(selectedClue1);
        currentClues.Remove(selectedClue2);
        usedClues.Add(selectedClue1);
        usedClues.Add(selectedClue2);
        UpdateClueList();
    }
}

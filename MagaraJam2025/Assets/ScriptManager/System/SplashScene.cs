using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private Canvas nameInputCanvas;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string sentence;
    [SerializeField] private float typeWait;
    public void Init()
    {

        nameInputCanvas.enabled = true;
        animator.SetTrigger("Start");
    }

    public IEnumerator WriteSentence()
    {
        text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typeWait);
        }
    }

    public void Continue()
    {
        DialogManager.instance.PlayerName = inputField.text;
        BackgroundManager.instance.DarkenScreen(1);
        StartCoroutine(wait(1));
        

    }

    private IEnumerator wait(float second)
    {
        yield return new WaitForSeconds(second);
        if (SahneManager.instance.currentScene.name != "MainScene")
            SahneManager.instance.LoadScene("MainScene");
    }
}

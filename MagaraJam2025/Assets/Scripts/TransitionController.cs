using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
public class TransitionController : MonoBehaviour
{
    
    public static TransitionController Instance { get; private set; }
    [SerializeField] private Image BlackImage;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void StartTransition(float time, Action onTransitionMidpoint = null)
    {
        StartCoroutine(TransitionCoroutine(time, onTransitionMidpoint));
    }
    private IEnumerator TransitionCoroutine(float time, Action onTransitionMidpoint)
    {
        BlackImage.color = new Color(0, 0, 0, 0);
        BlackImage.enabled = true;
        float elapsedTime = 0f;
        while (elapsedTime < time / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / (time / 2));
            BlackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        onTransitionMidpoint?.Invoke();

        elapsedTime = 0f;
        while (elapsedTime < time / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / (time / 2)));
            BlackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        BlackImage.enabled = false;
    }
}

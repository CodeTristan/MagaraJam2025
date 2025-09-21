using UnityEngine;
using UnityEngine.EventSystems;

public class NotebookAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform Notebook;

    public Vector3 OpenPosition;
    public Vector3 HalfOpenPosition;
    public Vector3 ClosedPosition;
    public float AnimationSpeed = 10f;

    private Vector3 TargetPosition;

    private bool isOpen = false;

    void Start()
    {
        TargetPosition = ClosedPosition;
        Notebook.anchoredPosition = ClosedPosition;
    }

    void Update()
    {
        Notebook.anchoredPosition = Vector3.Lerp(Notebook.anchoredPosition, TargetPosition, Time.deltaTime * AnimationSpeed);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isOpen)
            TargetPosition = HalfOpenPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isOpen)
            TargetPosition = ClosedPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TargetPosition = isOpen ? ClosedPosition : OpenPosition;
        isOpen = !isOpen;
        NotebookController.Instance.ToggleNotebook();
    }

    public void CloseNotebook()
    {
        TargetPosition = ClosedPosition;
        NotebookController.Instance.ToggleNotebook();
        isOpen = false;
    }
}

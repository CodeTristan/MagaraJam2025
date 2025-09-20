using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInformationText : MonoBehaviour
{

    [SerializeField] RectTransform ObjectTransform;
    [SerializeField] TextMeshProUGUI ObjectText;
    [SerializeField] Vector2 Offset;


    private Vector2 mousePos = Vector2.zero;

    private void Update()
    {
        if (Mouse.current != null)
        {
            mousePos = Mouse.current.position.ReadValue();
            TryRaycast(mousePos);
            ObjectTransform.position = mousePos + Offset;
        }
    }

    private void TryRaycast(Vector2 screenPos)
    {
        if (ClickRaycaster.Instance.CurrentCamera == null) return;

        Ray ray = ClickRaycaster.Instance.CurrentCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, ClickRaycaster.Instance.maxRayDistance, ClickRaycaster.Instance.interactableLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                SetMouseObject(screenPos, interactable.GetDescription());
            else
            {
                var parent = hit.collider.GetComponentInParent<IInteractable>();
                if (parent != null) SetMouseObject(screenPos, parent.GetDescription());
                else ClearMouseObject();
            }
        }
        else
        {
            ClearMouseObject();
        }
    }

    public void SetMouseObject(Vector2 position,string text)
    {
        ObjectTransform.gameObject.SetActive(true);
        ObjectText.gameObject.SetActive(true);

        ObjectText.text = text;
    }

    public void ClearMouseObject()
    {
        ObjectTransform.gameObject.SetActive(false);
        ObjectText.gameObject.SetActive(false);
        ObjectText.text = "";
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || ClickRaycaster.Instance.CurrentCamera == null) return;
        Ray r = ClickRaycaster.Instance.CurrentCamera.ScreenPointToRay(mousePos);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(r.origin, r.direction * ClickRaycaster.Instance.maxRayDistance);
    }
}

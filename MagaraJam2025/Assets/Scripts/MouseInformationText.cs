using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class MouseInformationText : MonoBehaviour
{
    [SerializeField] RectTransform ObjectTransform;
    [SerializeField] TextMeshProUGUI ObjectText;
    [SerializeField] Vector2 Offset;

    private Vector2 mousePos = Vector2.zero;

    private void Update()
    {
        if (Mouse.current == null) return;

        mousePos = Mouse.current.position.ReadValue();
        TryRaycast(mousePos);

        // Tooltip’in mouse’u takip etmesi
        ObjectTransform.position = mousePos + Offset;

        if(DialogManager.instance.inDialog)
            ClearMouseObject();
    }

    private void TryRaycast(Vector2 screenPos)
    {
        bool found = false;

        // --- 1) UI Raycast ---
        if (EventSystem.current != null)
        {
            var ped = new PointerEventData(EventSystem.current) { position = screenPos };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ped, results);

            //foreach (var r in results)
            //{
            //    if (r.gameObject.TryGetComponent<IInteractable>(out var uiInteractable))
            //    {
            //        SetMouseObject(screenPos, uiInteractable.GetDescription());
            //        found = true;
            //        break;
            //    }
            //}
            if(results.Count > 0)
            {
                if (results[0].gameObject.TryGetComponent<IInteractable>(out var uiInteractable))
                {
                    SetMouseObject(screenPos, uiInteractable.GetDescription());
                }
                else
                {
                    ClearMouseObject();
                }
                    found = true; // UI elementlerine týklandýysa 3D’ye geçme
            }
        }

        // --- 2) Eðer UI’de bulamazsak, 3D Physics Raycast ---
        if (!found && ClickRaycaster.Instance.CurrentCamera != null)
        {
            Ray ray = ClickRaycaster.Instance.CurrentCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit,
                ClickRaycaster.Instance.maxRayDistance,
                ClickRaycaster.Instance.interactableLayerMask,
                QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    SetMouseObject(screenPos, interactable.GetDescription());
                    found = true;
                }
                else
                {
                    var parent = hit.collider.GetComponentInParent<IInteractable>();
                    if (parent != null)
                    {
                        SetMouseObject(screenPos, parent.GetDescription());
                        found = true;
                    }
                }
            }
        }

        if (!found)
            ClearMouseObject();
    }

    public void SetMouseObject(Vector2 position, string text)
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

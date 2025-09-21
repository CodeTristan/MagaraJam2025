using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickRaycaster : MonoBehaviour
{
    public static ClickRaycaster Instance;

    public Camera CurrentCamera;
    public LayerMask interactableLayerMask;
    public float maxRayDistance = 100f;


    private Vector2 MouseClickPos = Vector2.zero;
    public void Init()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            HandleClick(pos);
        }

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                Vector2 pos = touch.position.ReadValue();
                HandleClick(pos);
            }
        }
    }

    private void HandleClick(Vector2 screenPos)
    {
        // --- Önce UI kontrolü ---
        PointerEventData ped = new PointerEventData(EventSystem.current) { position = screenPos };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        //foreach (var result in results)
        //{
        //    if (result.gameObject.TryGetComponent<IInteractable>(out var uiInteractable))
        //    {
        //        uiInteractable.OnInteract();
        //        return; // UI öncelikli, 3D’yi engellemek istersen burayý býrak
        //    }
        //}
        if(results.Count > 0)
        {
            results[0].gameObject.GetComponent<IInteractable>()?.OnInteract();
            return; // UI elementlerine týklandýysa 3D’ye geçme
        }

        // --- Sonra 3D kontrolü ---
        Ray ray = CurrentCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, interactableLayerMask))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var objInteractable))
                objInteractable.OnInteract();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (CurrentCamera == null || !Application.isPlaying) return;
        Ray r = CurrentCamera.ScreenPointToRay(MouseClickPos);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(r.origin, r.direction * maxRayDistance);
    }
}

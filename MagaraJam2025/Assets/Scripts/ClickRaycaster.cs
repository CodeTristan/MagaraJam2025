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
        if (EventSystem.current != null)
        {
            if (Mouse.current != null && EventSystem.current.IsPointerOverGameObject(-1))
                return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            MouseClickPos = Mouse.current.position.ReadValue();
            TryRaycast(MouseClickPos);
        }
    }

    private void TryRaycast(Vector2 screenPos)
    {
        if (CurrentCamera == null) return;

        Ray ray = CurrentCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, interactableLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                interactable.OnInteract();
            else
            {
                var parent = hit.collider.GetComponentInParent<IInteractable>();
                if (parent != null) parent.OnInteract();
            }
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

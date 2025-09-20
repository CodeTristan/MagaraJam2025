using UnityEngine;

public class LittleGameManager : MonoBehaviour    
{
    public static LittleGameManager Instance { get; private set; }

    [SerializeField] private GlobalRoomController globalRoomController;
    [SerializeField] private ClickRaycaster clickRaycaster;
    [SerializeField] private TransitionController transitionController;
    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        transitionController.Init();
        clickRaycaster.Init();
        globalRoomController.Init();
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GlobalRoomController globalRoomController;
    [SerializeField] private ClickRaycaster clickRaycaster;
    [SerializeField] private TransitionController transitionController;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

using System.Collections.Generic;
using UnityEngine;

public enum RoomName
{
    ArtistRoom,
    GalleryRoom,
}
public class GlobalRoomController : MonoBehaviour
{
    public static GlobalRoomController Instance { get; private set; }

    [SerializeField] private RoomController[] roomControllers;

    [SerializeField] private RectTransform RoomCameraContent;
    [SerializeField] private CameraButton[] RoomCameraButtons;


    private RoomController currentRoom;
    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var room in roomControllers)
        {
            room.Init();
        }

        OpenRoom(RoomName.ArtistRoom);
    }

    public void OpenRoom(RoomName roomName)
    {
        CloseAllCameraButtons();
        foreach (var room in roomControllers)
        {
            if (room.roomName == roomName)
            {
                room.OpenRoom();
                currentRoom = room;
                RoomCameraButtons[0].SetButtonActive(true);
                for (var i = 0; i < room.cameras.Length; i++)
                {
                    if (i < RoomCameraButtons.Length)
                    {
                        RoomCameraButtons[i].gameObject.SetActive(true);
                        RoomCameraButtons[i].LinkedCamera = room.cameras[i];
                        int index = i;

                        RoomCameraButtons[i].Button.onClick.RemoveAllListeners();
                        RoomCameraButtons[i].Button.onClick.AddListener(() => SetRoomCamera(room.cameras[index]));
                    }
                }
            }
        }
    }

    public void CloseAllCameraButtons()
    {
        foreach (var btn in RoomCameraButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    private void SetRoomCamera(Camera cam)
    {
        TransitionController.Instance.StartTransition(0.2f, () => SetRoomCameraAction(cam));
    }

    private void SetRoomCameraAction(Camera cam)
    {
        if (cam == null || currentRoom == null) return;

        ClickRaycaster.Instance.CurrentCamera = cam;
        currentRoom.CurrentCamera.enabled = false;
        foreach (var btn in RoomCameraButtons)
        {
            cam.enabled = true;
            currentRoom.CurrentCamera = cam;
            btn.SetButtonActive(false);
            if (btn.LinkedCamera == cam)
                btn.SetButtonActive(true);
        }
    }
}

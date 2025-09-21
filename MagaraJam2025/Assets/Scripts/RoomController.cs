using UnityEngine;

public class RoomController : MonoBehaviour
{
    public RoomName roomName;
    public Camera[] cameras;

    public Camera CurrentCamera;
    public PlaceSO placeSO;

    private Place place;
    public void Init()
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
        place = new Place(placeSO);
    }


    public void OpenRoom()
    {
        cameras[0].enabled = true;
        CurrentCamera = cameras[0];
        place.CheckDialogToTrigger();
    }
}

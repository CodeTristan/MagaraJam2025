using UnityEngine;

public class RoomController : MonoBehaviour
{
    public RoomName roomName;
    public Camera[] cameras;

    public Camera CurrentCamera;
    public void Init()
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
    }


    public void OpenRoom()
    {
        cameras[0].enabled = true;
        CurrentCamera = cameras[0];
    }
}

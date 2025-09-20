using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI Text;

    public Camera LinkedCamera;

    public void SetButtonActive(bool active)
    {
        if (active)
        {
            Button.interactable = false;
            Text.color = Color.yellow;
        }
        else
        {
            Button.interactable = true;
            Text.color = Color.white;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CanvasContainer
{
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public CanvasContainer(Canvas canvas, CanvasScaler canvasScaler)
    {
        this.canvas = canvas;
        this.canvasScaler = canvasScaler;
    }
}

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public List<CanvasContainer> Canvases;
    public void Init()
    {
        instance = this;
        AdjustCanvasScalers();
    }

    public void AdjustCameraScales()
    {
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            // letterbox
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // pillarbox
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
    public void AdjustCanvasScalers()
    {
        float ratio = (float)Screen.width / (float)Screen.height;

        foreach (var canvas in Canvases)
        {
            canvas.canvasScaler.matchWidthOrHeight = ratio > 16.0f / 9.0f ? 1 : 0;
        }
    }

    [ContextMenu("GetCanvases")]
    public void GetCanvases()
    {
        Canvases = new List<CanvasContainer>();

        Canvas[] canvasArray = FindObjectsOfType<Canvas>();
        foreach (var canvas in canvasArray)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                if (scaler != null)
                {
                    Canvases.Add(new CanvasContainer(canvas, scaler));
                }
            }
        }
    }
}

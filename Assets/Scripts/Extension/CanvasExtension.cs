using UnityEngine;

public static class CanvasExtension 
{
    public static Vector2 WorldToCanvas(this Canvas canvas, Vector3 position, Camera camera = null, Vector2 offset = default)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position) - offset;
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out result);
        return result;
    }
}

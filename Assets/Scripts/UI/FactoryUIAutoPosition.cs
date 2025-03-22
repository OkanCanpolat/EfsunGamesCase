#if UNITY_EDITOR
using UnityEngine;

[ExecuteAlways]
public class FactoryUIAutoPosition : MonoBehaviour
{
    [Header("UI Position")]
    [SerializeField] private Vector3 offset;
    private Canvas canvas;
    private FactoryBase factory;
    private Camera mainCamera;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        factory = GetComponentInParent<FactoryBase>();
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Application.isEditor && !Application.isPlaying)
        {
            if (factory.Config.ReferenceCameraDistance <= 0f) return;

            float distance = Vector3.Distance(mainCamera.transform.position, factory.transform.position);
            float scale = factory.Config.ReferenceCameraDistance / distance;
            Vector2 targetOffset = new Vector2(0, factory.Config.YOffset * scale);
            rectTransform.anchoredPosition = canvas.WorldToCanvas(factory.transform.position, mainCamera, targetOffset);
        }
    }

    [ContextMenu("Adjust UI Position")]
    public void AdjustUIPosition()
    {
        rectTransform.anchoredPosition = canvas.WorldToCanvas(factory.transform.position, Camera.main, offset);
        factory.Config.YOffset = offset.y;
        factory.Config.ReferenceCameraDistance = Vector3.Distance(Camera.main.transform.position, factory.transform.position);
    }
}

#endif


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class FactoryUIControllerBase : MonoBehaviour
{
    [Header("General" , order = 1)]
    protected FactoryBase factory;
    [SerializeField] protected Slider timeSlider;
    [SerializeField] protected TMP_Text storageText;
    [SerializeField] protected TMP_Text timeText;
    [SerializeField] protected Image outputImage;

    protected int factoryProductionTime;
    
    protected virtual void Awake()
    {
        factory = GetComponentInParent<FactoryBase>();

        factory.OnStorageChange += OnStorageChange;
        factory.OnTimeChange += OnTimeChange;
        factory.OnGenerationStart += OnGenerationStart;
        factory.OnGenerationEnd += OnGenerationEnd;
        factory.OnGenerate += OnGenerate;

        outputImage.sprite = factory.Config.OutputSprite;
        factoryProductionTime = factory.Config.ProductionTime;

        float distance = Vector3.Distance(Camera.main.transform.position, factory.transform.position);
        float scale = factory.Config.ReferenceCameraDistance / distance;
        Vector2 targetOffset = new Vector2(0, factory.Config.YOffset * scale);
        GetComponent<RectTransform>().anchoredPosition = GetComponentInParent<Canvas>().WorldToCanvas(factory.transform.position, Camera.main, targetOffset);
    }

    protected virtual void Start()
    {
        InitUI();
    }
    public abstract void OnStorageChange(int value);
    public abstract void OnTimeChange(int value);
    public abstract void OnGenerationStart();
    public abstract void OnGenerationEnd();
    public abstract void OnGenerate();
    public abstract void InitUI();
}

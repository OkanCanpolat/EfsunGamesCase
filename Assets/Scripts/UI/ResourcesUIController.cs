using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResourcesUIController : MonoBehaviour
{
    [Inject] private List<ResourceIndicator> resourceIndicators;
    [Inject] private Inventory inventory;
    [Inject] private ResourcesUIConfig uiConfig;
    private void Awake()
    {
        inventory.OnResouceAmountChange += OnResourceChange;
        inventory.OnResouceRemove += OnResourceChange;
        inventory.OnResouceAdd += OnResourceChange;
        inventory.OnResouceAdd += OnResourceAdd;
    }
    private void Start()
    {
        InitUI();
    }
    public ResourceIndicator GetIndicator(ResourceType type)
    {
        foreach (ResourceIndicator item in resourceIndicators)
        {
            if (item.Type == type)
            {
                return item;
            }
        }
        return null;
    }
    private void InitUI()
    {
        foreach (ResourceIndicator item in resourceIndicators)
        {
            int amount = inventory.GetResourceAmount(item.Type);
            item.AmountText.text = amount.ToString();
        }
    }
    private void OnResourceChange(ResourceType type, int amount)
    {
        foreach (ResourceIndicator item in resourceIndicators)
        {
            if (item.Type == type)
            {
                item.AmountText.text = amount.ToString();
            }
        }
    }
    private async void OnResourceAdd(ResourceType type, int amount)
    {
        ResourceIndicator resourceIndicator = GetIndicator(type);
        if (resourceIndicator == null || resourceIndicator.IsAnimating) return;
        resourceIndicator.IsAnimating = true;
        Image image = resourceIndicator.IndicatorImage;
        Vector3 initialScale = image.transform.localScale;
        Vector3 targetScale = initialScale * uiConfig.ScaleMultiplier;
        float animTime = uiConfig.AnimationTime;
        iTween.ScaleTo(image.gameObject, iTween.Hash("scale", targetScale, "time", animTime, "easetype", iTween.EaseType.easeInBack));
        iTween.ScaleTo(image.gameObject, iTween.Hash("scale", initialScale, "time", animTime, "delay", animTime, "easetype", iTween.EaseType.easeOutBack));
        await UniTask.Delay(TimeSpan.FromSeconds(animTime * 2f));
        resourceIndicator.IsAnimating = false;
    }
}

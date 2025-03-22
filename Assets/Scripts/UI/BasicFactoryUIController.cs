using UnityEngine;

public class BasicFactoryUIController : FactoryUIControllerBase
{
    public override void OnStorageChange(int value)
    {
        storageText.text = value.ToString();
    }
    public override void OnTimeChange(int value)
    {
        int time = factoryProductionTime - value;
        timeText.text = time.ToString();
        float currentSliderValue = (float)factory.ElapsedTime / factoryProductionTime;
        float targetSliderValue = (factory.ElapsedTime + 1f) / factoryProductionTime;
        iTween.ValueTo(gameObject, iTween.Hash("from", currentSliderValue, "to", targetSliderValue, "time", 1f, "onupdate", "ChangeSliderValue"));
    }
    public void ChangeSliderValue(float targetValue)
    {
        timeSlider.value = targetValue;
    }
    public override void OnGenerationStart()
    {
        timeSlider.value = 0f;
        timeText.text = factory.Config.ProductionTime.ToString();
    }
    public override void OnGenerationEnd()
    {
        timeText.text = "FULL";
        timeSlider.value = 1f;
    }
    public override void OnGenerate()
    {
    }
    public override void InitUI()
    {
        if (factory.CurrentResourceAmount >= factory.Config.Capacity)
        {
            timeText.text = "FULL";
            timeSlider.value = 1f;
        }
    }
}

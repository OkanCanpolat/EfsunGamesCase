using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AutomaticFactory : FactoryBase
{
    private void Start()
    {
        GenerateResource();
    }
    public override async void GenerateResource()
    {
        if (currentResourceAmount >= config.Capacity) return;

        OnGenerationStart?.Invoke();

        while (currentResourceAmount < config.Capacity)
        {

            while (elapsedTime < config.ProductionTime)
            {
                OnTimeChange?.Invoke(elapsedTime);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                elapsedTime++;
            }

            elapsedTime = 0;
            await UniTask.Yield();
            currentResourceAmount += config.OutputAmount;
            OnGenerate?.Invoke();
            OnStorageChange?.Invoke(currentResourceAmount);
        }

        OnGenerationEnd?.Invoke();
    }

    public override void OnClick()
    {
        base.OnClick();

        if (currentResourceAmount <= 0) return;

        audioService.PlaySoundOnce(soundConfig.CollectResourceSound, soundConfig.CollectResourceSoundVolume);
        int collectedAmount = currentResourceAmount;
        inventory.AddResource(config.Output, currentResourceAmount);
        currentResourceAmount = 0;
        OnStorageChange?.Invoke(currentResourceAmount);
        if (collectedAmount >= config.Capacity) GenerateResource();
    }

    public override void SavaData(GameData gameData)
    {
        FactoryData data;

        if (gameData.FactoryDataContainer.Factories.TryGetFactoryData(id, out data))
        {
            data.ElapsedTime = elapsedTime;
            data.CurrentStorageCount = currentResourceAmount;
        }
        else
        {
            data = new FactoryData(elapsedTime, id, currentResourceAmount);
            gameData.FactoryDataContainer.Factories.Add(data);
        }
    }

    public override void LoadData(GameData gameData)
    {
        FactoryData data;

        if (gameData.FactoryDataContainer.Factories.TryGetFactoryData(id, out data))
        {
            currentResourceAmount = data.CurrentStorageCount;
            elapsedTime = data.ElapsedTime;

            if (currentResourceAmount < config.Capacity)
            {
                float secondsSinceExit = timeController.ElapsedTime;
                float totalSeconds = secondsSinceExit + elapsedTime;
                int generatedResourceCount = (int)totalSeconds / config.ProductionTime;
                currentResourceAmount += generatedResourceCount;
                currentResourceAmount = Mathf.Clamp(currentResourceAmount, 0, config.Capacity);

                elapsedTime = currentResourceAmount < config.Capacity ? (int)totalSeconds % config.ProductionTime : 0;
            }

            OnStorageChange(currentResourceAmount);
        }
    }
}

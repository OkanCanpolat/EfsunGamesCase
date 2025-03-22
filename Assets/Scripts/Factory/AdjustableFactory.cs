using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AdjustableFactory : FactoryBase
{
    public Action OnAddQueue;
    public Action OnRemoveQueue;
    public Action Clicked;
    public Action<int> OnCollect;

    private bool isGenerating;
    private int generationQueueCount;
    private bool canCollect;
    public bool IsGenerationg => isGenerating;
    public int GenerationQueueCount => generationQueueCount;
    public bool CanCollect { get => canCollect; set => canCollect = value; }

    private void Start()
    {
    }
    public async override void GenerateResource()
    {
        OnGenerationStart?.Invoke();
        isGenerating = true;

        while (generationQueueCount > 0)
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
            generationQueueCount--;
            OnRemoveQueue?.Invoke();
            OnGenerate?.Invoke();
            OnStorageChange?.Invoke(currentResourceAmount);
        }

        OnGenerationEnd?.Invoke();
        isGenerating = false;
    }

    public override void OnClick()
    {
        base.OnClick();

        if (canCollect)
        {
            if (currentResourceAmount <= 0) return;
            audioService.PlaySoundOnce(soundConfig.CollectResourceSound, soundConfig.CollectResourceSoundVolume);
            inventory.AddResource(config.Output, currentResourceAmount);
            currentResourceAmount = 0;
            OnCollect?.Invoke(currentResourceAmount);
            OnStorageChange?.Invoke(currentResourceAmount);
        }

        Clicked?.Invoke();
    }

    public void AddQueue()
    {
        generationQueueCount++;
        inventory.RemoveResouce(config.Requirement.Type, config.Requirement.Amount);
        OnAddQueue?.Invoke();

        if (!isGenerating)
        {
            GenerateResource();
        }
    }
    public void RemoveQueue()
    {
        generationQueueCount--;
        inventory.AddResource(config.Requirement.Type, config.Requirement.Amount);
        OnRemoveQueue?.Invoke();
    }

    public override void SavaData(GameData gameData)
    {
        FactoryData data;

        if (gameData.FactoryDataContainer.Factories.TryGetFactoryData(id, out data))
        {
            data.ElapsedTime = elapsedTime;
            data.CurrentStorageCount = currentResourceAmount;
            data.QueueCount = generationQueueCount;
        }
        else
        {
            data = new FactoryData(elapsedTime, id, currentResourceAmount, generationQueueCount);
            gameData.FactoryDataContainer.Factories.Add(data);
        }
    }

    public override void LoadData(GameData gameData)
    {
        FactoryData data;

        if (gameData.FactoryDataContainer.Factories.TryGetFactoryData(id, out data))
        {
            currentResourceAmount = data.CurrentStorageCount;
            generationQueueCount = data.QueueCount;

            if (data.QueueCount > 0)
            {
                float secondsSinceExit = timeController.ElapsedTime;
                float totalSeconds = secondsSinceExit + data.ElapsedTime;
                int generatedResourceCount = (int)totalSeconds / config.ProductionTime;
                generatedResourceCount = Mathf.Clamp(generatedResourceCount, 0, data.QueueCount);
                currentResourceAmount += generatedResourceCount;

                if (generatedResourceCount < data.QueueCount)
                {
                    generationQueueCount -= generatedResourceCount;
                    elapsedTime = (int)totalSeconds % config.ProductionTime;
                    GenerateResource();
                }
                else
                {
                    generationQueueCount = 0;
                    elapsedTime = 0;
                }
            }
        }

        OnStorageChange(currentResourceAmount);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[Serializable]
public class GameData
{
    public DateTime ExitTime = DateTime.MinValue;
    public InventoryData InventoryData = new InventoryData();
    public FactoryDataContainer FactoryDataContainer = new FactoryDataContainer();
}

[Serializable]
public class InventoryData
{
    public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();
}

[Serializable]
public class FactoryDataContainer
{
    public List<FactoryData> Factories = new List<FactoryData>();
}

[Serializable]
public class FactoryData
{
    public int ElapsedTime;
    public string Id;
    public int CurrentStorageCount;
    public int QueueCount;
    public FactoryData(int elapsedTime, string id, int currentStorageCount, int queueCount = 0)
    {
        ElapsedTime = elapsedTime;
        Id = id;
        CurrentStorageCount = currentStorageCount;
        QueueCount = queueCount;
    }
}

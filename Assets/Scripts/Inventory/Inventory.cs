using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IDataPersistence
{
    private Dictionary<ResourceType, int> resources;
    private DataPersistenceManager persistenceManager;
    public Action<ResourceType, int> OnResouceAmountChange;
    public Action<ResourceType, int> OnResouceAdd;
    public Action<ResourceType, int> OnResouceRemove;

    public Dictionary<ResourceType, int> Resources => resources;
    public Inventory(DataPersistenceManager persistenceManager)
    {
        this.persistenceManager = persistenceManager;
        resources = new Dictionary<ResourceType, int>();
        persistenceManager.RegisterDataPersistence(this);
    }

    public int GetResourceAmount(ResourceType type)
    {
        if (resources.ContainsKey(type))
        {
            return resources[type];
        }

        return 0;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            int target = resources[type] + amount;
            resources[type] = target;
            OnResouceAdd?.Invoke(type, target);
        }
        else
        {
            resources.Add(type, amount);
            OnResouceAdd?.Invoke(type, amount);
        }
    }

    public void RemoveResouce(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            int target = resources[type] - amount;
            resources[type] = target;
            OnResouceRemove?.Invoke(type, target);
        }

        else
        {
            Debug.LogError("There is no type of " + type.ToString() + " in inventory");
        }
    }

    public void SavaData(GameData gameData)
    {
        Dictionary<ResourceType, int> saveData = gameData.InventoryData.Resources;
        saveData.Clear();

        foreach (KeyValuePair<ResourceType, int> item in resources)
        {
            saveData.Add(item.Key, item.Value);
        }
    }

    public void LoadData(GameData gameData)
    {
        Dictionary<ResourceType, int> loadData = gameData.InventoryData.Resources;
        foreach (KeyValuePair<ResourceType, int> item in loadData)
        {
            resources.Add(item.Key, item.Value);
            OnResouceAmountChange?.Invoke(item.Key, item.Value);
        }
    }
}

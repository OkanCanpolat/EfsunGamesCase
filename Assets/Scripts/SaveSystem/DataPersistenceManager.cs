using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DataPersistenceManager : MonoBehaviour
{
    private List<IDataPersistence> dataPersistences = new List<IDataPersistence>();
    [Inject] private IDataHandler dataHandler;
    private GameData gameData;
    public void RegisterDataPersistence(IDataPersistence dataPersistence)
    {
        dataPersistences.Add(dataPersistence);
    }

    private void Start()
    {
        LoadGame();
    }
    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            gameData = new GameData();
        }

        foreach(IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in dataPersistences)
        {
            dataPersistence.SavaData(gameData);
        }

        dataHandler.Save(gameData);
    }


    public void OnApplicationQuit()
    {
        SaveGame();
    }
}

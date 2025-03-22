using System;

public class GameTimeController : IDataPersistence
{
    private DateTime currentTime;
    private DataPersistenceManager persistenceManager;
    private float elapsedTime;

    public float ElapsedTime => elapsedTime;
    public GameTimeController(DataPersistenceManager persistenceManager)
    {
        this.persistenceManager = persistenceManager;
        persistenceManager.RegisterDataPersistence(this);
        currentTime = DateTime.Now;
    }
    public void LoadData(GameData gameData)
    {
        DateTime exitTime = gameData.ExitTime;
        if (exitTime == DateTime.MinValue) return;
        elapsedTime = (float)(currentTime - exitTime).TotalSeconds;
    }

    public void SavaData(GameData gameData)
    {
        gameData.ExitTime = DateTime.Now;
    }


}

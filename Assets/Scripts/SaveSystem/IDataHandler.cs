public interface IDataHandler
{
    public GameData Load();
    public void Save(GameData gameData);
}

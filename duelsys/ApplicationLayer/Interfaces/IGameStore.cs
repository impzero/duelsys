namespace duelsys.ApplicationLayer.Interfaces;

public interface IGameStore
{
    int SaveGame(Game g, int mId);
}
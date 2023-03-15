public interface IGameState
{
    public IGameState Tick(GameControlScript gcs, GameData data);
    public void Enter(GameControlScript gcs, GameData data);
    public void Exit(GameControlScript gcs);
}

namespace Source.Scripts.SaveSystem.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(IStorage storage);
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(IStorage storage);
    }
}
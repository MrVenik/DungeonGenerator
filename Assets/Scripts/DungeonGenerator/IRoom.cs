namespace DungeonGenerator
{
    public interface IRoom
    {
        Connection Connection { get; }
        bool CanCreate(int x, int y);
        void Create(int x, int y);
        void Build();
    }
}
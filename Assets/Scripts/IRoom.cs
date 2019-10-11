namespace DungeonGenerator
{
    public interface IRoom
    {
        Connection GetConnection();
        void Create(int x, int y);
        void Build();
    }
}
namespace DungeonGenerator
{
    internal class StartRoom : Room, IRoom
    {
        protected override void CreateConnections()
        {
            Connection = Connection.Start;
        }
    }
}
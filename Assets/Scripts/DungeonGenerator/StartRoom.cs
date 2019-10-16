namespace DungeonGenerator
{
    internal class StartRoom : TemplateRoom
    {
        protected override void CreateConnections()
        {
            Connection = Connection.Start;
        }
    }
}
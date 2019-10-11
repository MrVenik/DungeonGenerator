using System;

namespace DungeonGenerator
{
    public class BorderRoom : IRoom
    {
        public void Build()
        {

        }

        public void Create(int x, int y)
        {
            throw new Exception("Border room cant be created, it is used for borders of Dungeon array");
        }

        public Connection GetConnection()
        {
            return Connection.Border;
        }
    }
}
using System;

namespace DungeonGenerator
{
    public class BorderRoom : IRoom
    {
        public Connection Connection => Connection.Border;

        public void Build()
        {

        }

        public bool CanCreate(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Create(int x, int y)
        {
            throw new Exception("Border room cant be created, it is used for borders of Dungeon array");
        }
    }
}
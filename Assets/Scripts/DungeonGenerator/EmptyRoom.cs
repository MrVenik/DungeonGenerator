using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    public class EmptyRoom : IRoom
    {
        public Connection Connection => Connection.None;

        public void Build()
        {

        }

        public bool CanCreate(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Create(int x, int y)
        {
            throw new Exception("Empty room cant be created, it is used for filling Dungeon array");
        }
    }
}

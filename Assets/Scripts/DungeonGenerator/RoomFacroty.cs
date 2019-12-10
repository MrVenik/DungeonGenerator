using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public abstract class RoomFacroty : ScriptableObject
    {
        [SerializeField] private bool _shouldCreateNextRoom;

        public bool ShouldCreateNextRoom { get => _shouldCreateNextRoom; private set => _shouldCreateNextRoom = value; }

        public virtual void Create(int x, int y, RoomData roomData)
        {
            CreateNextRooms(x, y, roomData);
        }

        public abstract void CreateNextRooms(int x, int y, RoomData roomData);
        protected abstract void CreateNextRoom(int x, int y, Side side, RoomData roomData);
    }
}

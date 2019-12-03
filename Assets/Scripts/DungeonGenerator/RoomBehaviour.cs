using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public abstract class RoomBehaviour : MonoBehaviour
    {
        [SerializeField] private RoomSize _size;
        [SerializeField] protected List<RoomPrefabData> PossibleNextRooms;
        [SerializeField] private float _chanceOfNextRoom = 0.0f;
        [SerializeField] private bool ShouldCreateNextRoom = true;
        protected virtual float ChanceOfNextRoom
        {
            get => _chanceOfNextRoom;
        }

        [SerializeField] public Side Entrance;
        [SerializeField] private Connection _connection;
        public Connection Connection
        {
            get => _connection;
            set => _connection = value;
        }

        public RoomSize Size
        {
            get => _size;
            protected set => _size = value;
        }

        public Transform Transform { get; private set; }
        protected int X { get; set; }
        protected int Y { get; set; }

        private void Awake()
        {
            Transform = transform;
        }

        public abstract bool CanCreate(int x, int y);

        public abstract void Create(int x, int y);

        protected virtual void CreateNextRooms()
        {
            /*
            if (CanCreateNextRoom(Connection.Top)) CreateNextRoom(_x, _y + 1);
            if (CanCreateNextRoom(Connection.Bottom)) CreateNextRoom(_x, _y - 1);
            if (CanCreateNextRoom(Connection.Left)) CreateNextRoom(_x - 1, _y);
            if (CanCreateNextRoom(Connection.Right)) CreateNextRoom(_x + 1, _y);
            */
            if (ShouldCreateNextRoom)
            {
                var queue = new List<(int, int, Side)>();
                if (CanCreateNextRoom(Connection.Top)) queue.Add((X, Y + 1, Side.Top));
                if (CanCreateNextRoom(Connection.Bottom)) queue.Add((X, Y - 1, Side.Bottom));
                if (CanCreateNextRoom(Connection.Left)) queue.Add((X - 1, Y, Side.Left));
                if (CanCreateNextRoom(Connection.Right)) queue.Add((X + 1, Y, Side.Right));
                queue.Shuffle();
                foreach (var room in queue)
                {
                    CreateNextRoom(room.Item1, room.Item2, room.Item3);
                }
            }
        }

        protected virtual bool CanCreateNextRoom(ConnectionType type)
        {
            return type != ConnectionType.Wall
                && type != ConnectionType.None
                && type != ConnectionType.Border;
        }

        protected virtual void CreateNextRoom(int x, int y, Side side)
        {
            if (ShouldCreateNextRoom)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);
                if (chance <= ChanceOfNextRoom)
                {
                    RoomCreator.Create(x, y, side, PossibleNextRooms);
                } 
            }
        }

        public abstract void Build();
    }
}
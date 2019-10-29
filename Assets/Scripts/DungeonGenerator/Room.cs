using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public enum Side
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public abstract class Room : MonoBehaviour, IRoom
    {
        [SerializeField] protected List<RoomPrefabData> PossibleNextRooms;
        [SerializeField] public Side Entrance;
        [SerializeField] private Connection _connection;
        public Connection Connection
        {
            get => _connection;
            set => _connection = value;
        }

        public Transform Transform { get; private set; }

        protected int _x, _y;

        private void Awake()
        {
            Transform = transform;
        }

        public abstract bool CanCreate(int x, int y);

        public abstract void Create(int x, int y);

        protected abstract void CreateConnections();

        protected virtual void CreateNextRooms()
        {
            /*
            if (CanCreateNextRoom(Connection.Top)) CreateNextRoom(_x, _y + 1);
            if (CanCreateNextRoom(Connection.Bottom)) CreateNextRoom(_x, _y - 1);
            if (CanCreateNextRoom(Connection.Left)) CreateNextRoom(_x - 1, _y);
            if (CanCreateNextRoom(Connection.Right)) CreateNextRoom(_x + 1, _y);
            */

            var queue = new List<(int, int, Side)>();
            if (CanCreateNextRoom(Connection.Top)) queue.Add((_x, _y + 1, Side.Top));
            if (CanCreateNextRoom(Connection.Bottom)) queue.Add((_x, _y - 1, Side.Bottom));
            if (CanCreateNextRoom(Connection.Left)) queue.Add((_x - 1, _y, Side.Left));
            if (CanCreateNextRoom(Connection.Right)) queue.Add((_x + 1, _y, Side.Right));
            queue.Shuffle();
            foreach (var room in queue)
            {
                CreateNextRoom(room.Item1, room.Item2, room.Item3);
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
            RoomSpawnPoint.Spawn(x, y, PossibleNextRooms, side);
        }

        public abstract void Build();
    }
}
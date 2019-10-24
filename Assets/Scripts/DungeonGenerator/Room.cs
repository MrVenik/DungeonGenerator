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

        protected abstract void CreateNextRooms();

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
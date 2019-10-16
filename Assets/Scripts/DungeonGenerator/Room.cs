using System;
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
        [SerializeField] private Connection _connection;
        public Connection Connection
        {
            get
            {
                return _connection;
            }
            protected set
            {
                _connection = value;
            }
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

        protected abstract void CreateNextRoom(int x, int y);

        public abstract void Build();
    }
}
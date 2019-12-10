using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class RoomBehaviour : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private Connection _connection;
        [SerializeField] private Side _entrance;
        public Connection Connection { get => _connection; set => _connection = value; }
        public Side Entrance { get => _entrance; set => _entrance = value; }
        public int ID { get => _id; set => _id = value; }
    }
}
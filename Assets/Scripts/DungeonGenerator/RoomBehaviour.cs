using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class RoomBehaviour : MonoBehaviour
    {
        [SerializeField] private Connection _connection;
        public Connection Connection { get => _connection; set => _connection = value; }
    }
}
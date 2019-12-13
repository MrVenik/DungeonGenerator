using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class RoomData : CreatableData
    {
        [SerializeField] private string _name;
        [SerializeField] private int _id;
        [SerializeField] private RoomSize _size;
        [SerializeField] private Side _entrance;
        [SerializeField] private Connection _connection;
        [SerializeField] private bool _shouldCreateNextRoom;

        [SerializeField] private RoomChecker _checker;
        [SerializeField] private RoomBuilder _builder;
        [SerializeField] private RoomCreator _creator;

        public string Name { get => _name; private set => _name = value; }
        public Connection Connection { get => _connection; private set => _connection = value; }
        public Side Entrance { get => _entrance; private set => _entrance = value; }
        public RoomSize Size { get => _size; private set => _size = value; }
        public RoomChecker Checker { get => _checker; private set => _checker = value; }
        public RoomBuilder Builder { get => _builder; private set => _builder = value; }
        public bool ShouldCreateNextRoom { get => _shouldCreateNextRoom; private set => _shouldCreateNextRoom = value; }
        public RoomCreator Creator { get => _creator; private set => _creator = value; }
        public override bool IsPlug { get => !ShouldCreateNextRoom; }
        public int ID { get => _id; set => _id = value; }
        public bool Created { get; protected set; }
        public bool IsCreatingNextRooms { get; protected set; }

        public override bool CanCreate(int x, int y) => Checker.CanCreate(x, y, this);
        public override void Create(int x, int y) => Create(x, y);
        public void Build(int x, int y, Transform transform) => Builder.Build(x, y, this, transform);

        public override void Rotate(Side side)
        {
            switch (Entrance)
            {
                case Side.Top:
                    switch (side)
                    {
                        case Side.Top:
                            break;
                        case Side.Bottom:
                            Rotate(2);
                            break;
                        case Side.Left:
                            Rotate(1, false);
                            break;
                        case Side.Right:
                            Rotate(1, true);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Bottom:
                    switch (side)
                    {
                        case Side.Top:
                            Rotate(2);
                            break;
                        case Side.Bottom:
                            break;
                        case Side.Left:
                            Rotate(1, true);
                            break;
                        case Side.Right:
                            Rotate(1, false);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Left:
                    switch (side)
                    {
                        case Side.Top:
                            Rotate(1, true);
                            break;
                        case Side.Bottom:
                            Rotate(1, false);
                            break;
                        case Side.Left:
                            break;
                        case Side.Right:
                            Rotate(2);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Right:
                    switch (side)
                    {
                        case Side.Top:
                            Rotate(1, false);
                            break;
                        case Side.Bottom:
                            Rotate(1, true);
                            break;
                        case Side.Left:
                            Rotate(2);
                            break;
                        case Side.Right:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Rotate(int steps, bool clockwise = false)
        {
            for (int i = 0; i < steps; i++)
            {
                Rotate(clockwise);
            }
        }

        public void Rotate(bool clockwise = false)
        {
            Connection = Connection.Rotate(clockwise);
            Entrance = Entrance.Rotate(clockwise);
        }
    }
}

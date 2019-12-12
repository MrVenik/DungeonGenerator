using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class DungeonManager : MonoBehaviour
    {
        public static Dungeon Dungeon { get; private set; }

        [SerializeField] private GameObject _dungeonPrefab;
        [SerializeField] private int _currentAmountOfRooms;
        [SerializeField] private bool _useDeviation;

        [SerializeField] private CreatableData _exampleRoomForReservation;
        [SerializeField] private CreatableData _exampleBossRoom;
        private void Awake()
        {
            CreateAndBuild();
            ScriptableObject[] objects = Resources.FindObjectsOfTypeAll<CreatableData>();
            foreach (var item in objects)
            {
                Debug.Log(item);
            }
        }

        public void CreateAndBuild()
        {
            CreateDungeon();
            BuildDungeon();
        }

        public void CreateDungeon()
        {
            if (Dungeon != null)
            {
                Destroy(Dungeon.gameObject);
            }

            Debug.Log(System.DateTime.Now);
            Dungeon = Instantiate(_dungeonPrefab, transform.position, transform.rotation, transform).GetComponent<Dungeon>();

            int rndX = UnityEngine.Random.Range(1, Dungeon.Width - 1);
            int rndY = UnityEngine.Random.Range(1, Dungeon.Heigth - 1);

            List<Side> sides = new List<Side>
            {
                Side.Top,
                Side.Bottom,
                Side.Left,
                Side.Right
            };

            Side rndSide = sides[UnityEngine.Random.Range(0, sides.Count)];

            //Dungeon.ReserveRoom(1, 1, Side.Bottom, _exampleRoomForReservation);
            Dungeon.ReserveRoom(25, 15, Side.Top, _exampleRoomForReservation);

            Dungeon.BuildPath(15, 1, Side.Top, 25, 15, Side.Bottom);

            Dungeon.ReserveRoom(15, 25, Side.Top, _exampleBossRoom);

            Dungeon.BuildPath(25, 15, Side.Top, 15, 25, Side.Bottom);

            Dungeon.CreateStartRoom(15, 1, Side.Top);
            if (_useDeviation)
            {
                if (Math.Abs(Dungeon.PredicatedAmountOfRooms - Dungeon.AmountOfRooms) > Dungeon.AmountOfRooms * Dungeon.MaximumDeviation)
                {
                    CreateDungeon();
                }
            }

            Debug.Log(System.DateTime.Now);

            _currentAmountOfRooms = Dungeon.AmountOfRooms;
        }

        public void BuildDungeon()
        {
            if (Dungeon != null)
            {
                Dungeon.BuildDungeon();
            }
        }
    }
}

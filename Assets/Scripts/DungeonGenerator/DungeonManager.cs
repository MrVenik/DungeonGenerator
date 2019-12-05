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

        private void Awake()
        {
            CreateDungeon();
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

            Dungeon.CreateStartRoom(rndX, rndY, rndSide);
            Dungeon.BuildDungeon();
            Debug.Log(System.DateTime.Now);
        }
    }
}

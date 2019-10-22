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
            Debug.Log(System.DateTime.Now);
            Dungeon = Instantiate(_dungeonPrefab, transform.position, transform.rotation, transform).GetComponent<Dungeon>();
            Dungeon.CreateStartRoom(0, 0);
            Dungeon.BuildDungeon();
            Debug.Log(System.DateTime.Now);
        }
    }
}

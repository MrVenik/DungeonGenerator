using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DungeonGenerator.Editor
{
    [CustomEditor(typeof(DungeonManager))]
    public class DungeonManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DungeonManager dungeonManager = (DungeonManager)target;
            if (GUILayout.Button("Create and Build Dungeon"))
            {
                dungeonManager.CreateAndBuild();
            }
            if (GUILayout.Button("Create Dungeon"))
            {
                dungeonManager.CreateDungeon();
            }
            if (GUILayout.Button("Build Dungeon"))
            {
                dungeonManager.BuildDungeon();
            }
            if (GUILayout.Button("Start"))
            {
                dungeonManager.SetPlayer();
            }
        }
    }
}

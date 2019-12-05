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

            DungeonManager myScript = (DungeonManager)target;
            if (GUILayout.Button("Create Dungeon"))
            {
                myScript.CreateDungeon();
            }
        }
    }
}

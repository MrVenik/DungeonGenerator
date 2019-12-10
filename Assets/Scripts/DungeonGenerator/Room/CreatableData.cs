using UnityEngine;

namespace DungeonGenerator
{
    public abstract class CreatableData : ScriptableObject
    {
        public abstract bool IsPlug { get; }

        public abstract bool CanCreate(int x, int y);
        public abstract void Create(int x, int y);
        public abstract void Rotate(Side side);
    }
}
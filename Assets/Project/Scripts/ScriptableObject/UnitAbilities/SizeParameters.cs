
using UnityEngine;

namespace Project.Scripts.ScriptableObject.UnitAbilities
{
    [CreateAssetMenu(fileName = "Unit Size SO", menuName = "Unit Size SO", order = 53)]
    public class SizeParameters : UnityEngine.ScriptableObject
    {
        public UnitSize unitSize;
        public Vector3 unitScale;
        public double attackPoints;
        public double healthPoints;
        public double movementSpeed;
        public double attackSpeed;
    }
}

public enum UnitSize
{
    SMALL,
    BIG
}

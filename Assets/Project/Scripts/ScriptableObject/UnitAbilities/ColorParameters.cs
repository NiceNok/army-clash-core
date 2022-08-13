using UnityEngine;

namespace Project.Scripts.ScriptableObject.UnitAbilities
{
    [CreateAssetMenu(fileName = "Unit Color SO", menuName = "Unit Color SO", order = 52)]
    public class ColorParameters : UnityEngine.ScriptableObject
    {
        public UnitColor color;
        public Material coloredMaterial;
        public Material coloredMaterialEnemy;
        public double attackPoints;
        public double healthPoints;
        public double movementSpeed;
        public double attackSpeed;
    }

    public enum UnitColor
    {
        BLUE = 0,
        GREEN = 1,
        RED = 2
    }
}
using System;
using Project.Scripts.ScriptableObject.UnitAbilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Scripts.Units
{
    public class MeleeSoldier : Unit
    {
        private float meleeRangeDistance = 2f;
        [SerializeField] private ObjectPool unitHealthFXPool;

        private void Awake()
        {
            Assert.IsNotNull(unitHealthFXPool);
            
            unitHealthFXPool.Initialize();
        }

        Unit FindClosestEnemy()
        {
            Unit closestUnit = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;

            foreach (Unit unit in enemies)
            {
                if (!unit.isActiveAndEnabled) continue;
                float dist = Vector3.Distance(unit.transform.position, currentPos);
                if (dist < minDist)
                {
                    closestUnit = unit;
                    minDist = dist;
                }
            }

            return closestUnit;
        }

        Unit FindPoorEnemy()
        {
            if(IsMeleeDistance()) return currentEnemy;
            Unit poorUnit = null;
            double minHealth = Double.MaxValue;
            foreach (Unit unit in enemies)
            {
                if (!unit.isActiveAndEnabled) continue;
                double health = unit.HealthPoints;
                if (health < minHealth)
                {
                    poorUnit = unit;
                    minHealth = health;
                }
            }

            return poorUnit;
        }

        public override void GetDamage(double damage)
        {
            ChangeHealth(-damage);
            ShowHealthChange(damage);
            currentEnemy = FindClosestEnemy();
            if (IsDied) PerformDeath();
        }

        void ShowHealthChange(double damage)
        {
            var obj = unitHealthFXPool.GetObject();
            var hp = obj.GetComponent<UnitHealthChangeView>();
            hp.SetText($"-{(int)damage}");
            hp.OnFinish += ReturnHealthViewObject;
        }

        void ReturnHealthViewObject(UnitHealthChangeView hp)
        {
            unitHealthFXPool.ReturnObject(hp.gameObject);
            hp.OnFinish -= ReturnHealthViewObject;
        }

        protected override void MoveToEnemy()
        {
            currentEnemy = Type == UnitShape.CUBE ? FindClosestEnemy() : FindPoorEnemy();
            if (currentEnemy == null || isAttack || IsMeleeDistance()) return;
            var step = MovementSpeed / 2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentEnemy.transform.position, (float)step);
        }

        bool IsMeleeDistance()
        {
            if (!currentEnemy) return false;
            var dist = Vector3.Distance(currentEnemy.transform.position, transform.position);
            if (dist > meleeRangeDistance) return false;
            return true;
        }

        public override void PerformAttack()
        {
            if (!IsMeleeDistance()) return;
            currentEnemy.GetDamage(AttackPoints);
        }

        private new void PerformDeath()
        {
            CancelInvoke("PerformAttack");
            base.PerformDeath();
        }
    }
}
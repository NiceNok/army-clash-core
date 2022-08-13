using System;
using Project.Scripts.ScriptableObject.UnitAbilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Scripts.Units
{
    public class MeleeSoldier : Unit
    {
        [SerializeField] private ObjectPool unitHealthFXPool;

        private void Awake()
        {
            Assert.IsNotNull(unitHealthFXPool);
            
            unitHealthFXPool.Initialize();
        }

        Transform FindClosestEnemy()
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;

            foreach (Unit t in enemies)
            {
                if (!t.gameObject.activeSelf) continue;
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }

            return tMin;
        }

        Transform FindPoorEnemy()
        {
            Transform poorUnit = null;
            double minHealth = Double.MaxValue;
            foreach (Unit unit in enemies)
            {
                if (!unit.gameObject.activeSelf) continue;
                double health = unit.HealthPoints;
                if (health < minHealth)
                {
                    poorUnit = unit.transform;
                    minHealth = health;
                }
            }

            return poorUnit;
        }

        public override void GetDamage(double damage)
        {
            ChangeHealth(-damage);
            ShowHealthChange(damage);
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
            var closestEnemy = Type == UnitShape.CUBE ? FindClosestEnemy() : FindPoorEnemy();
            if (closestEnemy == null || isAttack || currentEnemy) return;
            var step = MovementSpeed / 2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.position, (float)step);
        }

        public override void PerformAttack()
        {
            currentEnemy.GetDamage(AttackPoints);
            if (currentEnemy.IsDied)
            {
                isAttack = false;
                currentEnemy = null;
                CancelInvoke("PerformAttack");
            }
        }

        private new void PerformDeath()
        {
            isAttack = false;
            currentEnemy = null;
            CancelInvoke("PerformAttack");
            base.PerformDeath();
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag(enemyTag) &&
                currentEnemy == null)
            {
                //Debug.LogError("ENTER");
                isAttack = true;
                currentEnemy = collision.gameObject.GetComponent<Unit>();
                InvokeRepeating("PerformAttack", (float)AttackSpeed,  (float)AttackSpeed);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag(enemyTag) &&
                currentEnemy == null )
            {
                //Debug.LogError("EXIT");
                isAttack = false;
                currentEnemy = null;
                CancelInvoke("PerformAttack");
            }
        }
    }
}
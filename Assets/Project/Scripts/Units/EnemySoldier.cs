/*
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Units
{
    public class EnemySoldier : Unit
    {

        private void OnDisable()
        {
            soldierMesh = null;
            isInited = false;
            Destroy(shape);
        }

        public void SetEnemies(List<MeleeSoldier> enemies)
        {
            this.enemies = new Transform[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
                this.enemies[i] = enemies[i].gameObject.transform;
        }

        void FindEnemy()
        {
        }

        Transform GetClosestEnemy(Transform[] enemies)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (Transform t in enemies)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }

        public override void GetDamage(double damage)
        {
            healthPoints -= damage;
            if (healthPoints <= 0)
            {
                //Die
            }
        }

        public override void MoveToEnemy()
        {
            var closestEnemy = GetClosestEnemy(enemies);
            if (closestEnemy == null) return;
            var step = (float)(movementSpeed/ 10) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.position, step);
        }
        
        public override void PerformAttack()
        {
            throw new System.NotImplementedException();
        }
    }
}
*/

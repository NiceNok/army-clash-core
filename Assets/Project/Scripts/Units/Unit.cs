using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.ScriptableObject.UnitAbilities;
using UnityEngine;

namespace Project.Scripts.Units
{
    public abstract class Unit: MonoBehaviour
    {
        [Header("Unit Characteristics")]
        [SerializeField] private double attackPoints;
        [SerializeField] private double healthPoints;
        [SerializeField] private double movementSpeed;
        [SerializeField] private double attackSpeed;
        public double AttackPoints => attackPoints;
        public double HealthPoints => healthPoints;
        public double MovementSpeed => movementSpeed;
        public double AttackSpeed => attackSpeed;

        [Header("Unit Inspector Dependency")]
        private MeshRenderer _soldierMesh;
        private GameObject _shape;
        private Rigidbody _rigidbody;
        protected UnitShape Type { get; private set; }

        [Header("Runtime Values")]
        public Unit[] enemies;
        public bool isAttack;
        public bool isInited;
        public bool IsDied => healthPoints <= 0;
        public Unit currentEnemy;
        public string enemyTag;

        public event Action<GameObject> OnDeath;
        
        internal void ChangeDamage(double attack) => attackPoints += attack;
        
        internal void ChangeHealth(double health) => healthPoints += health;

        internal void ChangeSpeed(double speed) => movementSpeed += speed;
        
        internal void ChangeAttackSpeed(double attackSpeed) => this.attackSpeed += attackSpeed;

        private void OnEnable()
        {
            StartCoroutine(StartAttack());
        }

        internal void Init(ShapeParameters shape, ColorParameters color , SizeParameters size)
        {
            SetBaseCharacteristics();
            if (_shape) Destroy(_shape);
            if (_soldierMesh) _soldierMesh = null;
            
            _shape = GameObject.CreatePrimitive(shape.type);
            _shape.transform.SetParent(gameObject.transform);
            _shape.transform.localPosition= new Vector3(0,0,0);
            _soldierMesh = _shape.gameObject.GetComponent<MeshRenderer>();
            _soldierMesh.material = color.coloredMaterial;
            Type = shape.unitShape;
            if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();

            gameObject.transform.localScale = size.unitScale;
            ChangeDamage(size.attackPoints + shape.attackPoints + color.attackPoints);
            ChangeHealth(size.healthPoints + shape.healthPoints + color.healthPoints);
            ChangeSpeed(size.movementSpeed + shape.movementSpeed + color.movementSpeed);
            ChangeAttackSpeed(size.attackSpeed + shape.attackSpeed + color.attackSpeed);

            isInited = true;
        }

        IEnumerator StartAttack()
        {
            yield return new WaitUntil(() => GameManager.Battle);
            
            InvokeRepeating("PerformAttack", (float)attackSpeed, (float)attackSpeed);
        }

        void FixedUpdate()
        {
            if (!isInited || !GameManager.Battle) return;
            MoveToEnemy();
            _rigidbody.velocity = Vector3.zero;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);//freeze rotation manually
        }

        private void OnDisable()
        {
            _soldierMesh = null;
            isInited = false;
            Destroy(_shape);
        }

        public void SetEnemyView(Material mat)
        {
            _soldierMesh.material = mat;
        }

        public void SetBaseCharacteristics()
        {
            attackPoints = Constants.BaseAttack;
            healthPoints = Constants.BaseHealthPoints;
            movementSpeed = Constants.BaseMovementSpeed;
            attackSpeed = Constants.BaseAttackSpeed;
        }
        
        public void SetEnemies(List<Unit> enemies, string enemyTag)
        {
            this.enemyTag = enemyTag;
            this.enemies = new Unit[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
                this.enemies[i] = enemies[i];
        }

        protected void PerformDeath() => OnDeath?.Invoke(gameObject);
        
        protected abstract void MoveToEnemy();
        
        public abstract void GetDamage(double damage);
        
        public abstract void PerformAttack();
        
    }
}

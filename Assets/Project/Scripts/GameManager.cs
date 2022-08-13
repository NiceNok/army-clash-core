using System.Collections.Generic;
using Project.Scripts.ScriptableObject.UnitAbilities;
using Project.Scripts.UI;
using Project.Scripts.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ObjectPool mySoldiersPool;
        [SerializeField] private ObjectPool enemySoldiersPool;

        [SerializeField] private ColorParameters[] colors;
        [SerializeField] private ShapeParameters[] shapes;
        [SerializeField] private SizeParameters[] size;

        [SerializeField] private UIManager uiManager;
        
        [Header("Army Values (base)")]
        [Space(15)]
        public int MyArmySize = 20;
        public int EnemyArmySize = 20;
        public float DistanceBetweenUnits = 1.6f;
        public int GridWidth = 5; 
        
        private readonly List<Unit> _mySoldiers = new List<Unit>();
        private readonly List<Unit> _enemySoldiers = new List<Unit>();

        public static bool Battle;

        private void Awake()
        {
            Assert.IsNotNull(mySoldiersPool);
            Assert.IsNotNull(enemySoldiersPool);
            Assert.IsNotNull(colors);
            Assert.IsNotNull(shapes);
            Assert.IsNotNull(size);
            Assert.IsNotNull(uiManager);
            
            Init();
        }

        void Init()
        {
            mySoldiersPool.Initialize();
            enemySoldiersPool.Initialize();
        }

        void Start()
        {
            for (int i = 0; i < MyArmySize; i++)
            {
                var obj = mySoldiersPool.GetObject();
                var soldier = obj.GetComponent<MeleeSoldier>();
                _mySoldiers.Add(soldier);
            }
            
            for (int i = 0; i < EnemyArmySize; i++)
            {
                var obj = enemySoldiersPool.GetObject();
                var soldier = obj.GetComponent<MeleeSoldier>();
                _enemySoldiers.Add(soldier);
            }

            RandomizeSoldiers();
        }

        void RandomizeSoldiers()
        {
            for (int i = 0; i < _mySoldiers.Count; i++)
            {
                var randomShape = shapes[Random.Range(0, shapes.Length)];
                var randomColor = colors[Random.Range(0, colors.Length)];
                var randomSize = size[Random.Range(0, size.Length)];
                SetGridPosition(_mySoldiers[i].transform, i);
                _mySoldiers[i].Init(randomShape, randomColor, randomSize);
                _mySoldiers[i].SetEnemies(_enemySoldiers, Constants.EnemyTag);
                _mySoldiers[i].OnDeath += ReturnSoldierToPool;
            }

            for (int i = 0; i < _enemySoldiers.Count; i++)
            {
                var randomShape = shapes[Random.Range(0, shapes.Length)];
                var randomColor = colors[Random.Range(0, colors.Length)];
                var randomSize = size[Random.Range(0, size.Length)];
                SetGridPosition(_enemySoldiers[i].transform, i);
                _enemySoldiers[i].Init(randomShape, randomColor, randomSize);
                _enemySoldiers[i].SetEnemies(_mySoldiers, Constants.FriendlyTag);
                _enemySoldiers[i].SetEnemyView(randomColor.coloredMaterialEnemy);
                _enemySoldiers[i].OnDeath += ReturnEnemyToPool;

            }
        }

        void ReturnSoldierToPool(GameObject obj)
        {
            mySoldiersPool.ReturnObject(obj);
            CheckGameState();
        }
        
        void ReturnEnemyToPool(GameObject obj)
        {
            enemySoldiersPool.ReturnObject(obj);
            CheckGameState();
        }

        void CheckGameState()
        {
            float aliveUnitsCount = 0;
            float aliveEnemyCount = 0;
            foreach (var soldier in _mySoldiers)
                if (soldier.gameObject.activeInHierarchy) aliveUnitsCount++;
            
            foreach (var soldier in _enemySoldiers)
                if (soldier.gameObject.activeInHierarchy) aliveEnemyCount++;
            
            uiManager.RefreshView(
                aliveUnitsCount/ _mySoldiers.Count, 
                aliveEnemyCount / _enemySoldiers.Count);
            
            if (aliveEnemyCount <= 0)
                EndGame(true);
            else if (aliveUnitsCount <= 0)
                EndGame(false);
                
        }

        void EndGame(bool state)
        {
            Battle = false;
            uiManager.OpenEndPanel(state);
        }

        public void RandomizeArmyButton()
        {
            foreach (var soldier in _mySoldiers)
                soldier.OnDeath -= ReturnEnemyToPool;
            
            foreach (var soldier in _enemySoldiers)
                soldier.OnDeath -= ReturnEnemyToPool;
            
            RandomizeSoldiers();
        }

        public void StartButton()
        {
            Battle = true;
            uiManager.EnableBottomPanel(false);
        }

        void SetGridPosition(Transform tr, int id)
        {
            var z = id % GridWidth * DistanceBetweenUnits;
            var x = id / GridWidth * DistanceBetweenUnits;
            tr.localPosition = new Vector3(x, 0, z);
        }
    }
}

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Top Panel")]
        [SerializeField] private Slider myUnitsProgressBar;
        [SerializeField] private Slider enemyUnitsProgressBar;

        [Header("BottomPanel")] 
        [SerializeField] private GameObject bottomPanel;
        
        [SerializeField] private EndGamePanel endGamePanel;


        private void Awake()
        {
            Assert.IsNotNull(myUnitsProgressBar);
            Assert.IsNotNull(enemyUnitsProgressBar);
            Assert.IsNotNull(bottomPanel);
            Assert.IsNotNull(endGamePanel);
        }

        public void RefreshView(float myUnits, float enemyUnits)
        {
            myUnitsProgressBar.value = myUnits;
            enemyUnitsProgressBar.value = enemyUnits;
        }

        public void OpenEndPanel(bool state)
        {
            endGamePanel.OpenEndPanel(state);
        }

        public void EnableBottomPanel(bool state)
        {
            bottomPanel.SetActive(state);
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(Constants.MenuSceneName);
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene(Constants.GameSceneName);
        }
    }
}
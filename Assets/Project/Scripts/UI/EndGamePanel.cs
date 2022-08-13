using UnityEngine;

namespace Project.Scripts.UI
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private GameObject win;
        [SerializeField] private GameObject lose;

        public void OpenEndPanel(bool state)
        {
            endGamePanel.SetActive(true);
            win.SetActive(state);
            lose.SetActive(!state);
        }
    }
}

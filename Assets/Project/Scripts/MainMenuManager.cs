using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(Constants.GameSceneName);
        }
    }
}

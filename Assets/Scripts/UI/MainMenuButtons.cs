using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuButtons : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}

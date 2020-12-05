using Gameplay;
using UnityEngine;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public void OnRestart()
        {
            _gameManager.Restart();
        }

        public void OnBackToMenu()
        {
            _gameManager.BackToMenu();
        }
    }
}

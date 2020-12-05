﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameObject currentPlayerGameObject;
        public GameObject GetCurrentPlayerGameObject => currentPlayerGameObject;

        private bool _gameOver = false;
        private bool _gameRunning = true;

        public bool IsGameOver => _gameOver;
        public bool IsGameRunning => _gameRunning;

        [SerializeField] private GameObject gameOverScreenGameObject;

        private void Awake()
        {
            if (Instance) DestroyImmediate(gameObject);

            Instance = this;
        }

        private void Start()
        {
            SpawnManager.Instance.SpawnWave();
        }

        public void GameOver()
        {
            _gameOver = true;
            _gameRunning = true;

            gameOverScreenGameObject.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        public void BackToMenu()
        {
            Debug.Log("GameManager:BackToMenu: Not Implemented");
            Restart();
        }
    }
}

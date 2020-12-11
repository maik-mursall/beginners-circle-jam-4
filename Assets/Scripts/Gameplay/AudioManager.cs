﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using Enemy;
using Combat;
using Player;


namespace Gameplay
{
    public class AudioManager : MonoBehaviour
    {
        public enum _comments { WIN, QUICK, VERLOREN, DIALOG };
        public Animator anim;
        private AudioSource _audioSource;

        public AudioClip[] _gewonnen ;
        public AudioClip[] _quick;
        public AudioClip[] _kampf;
        public AudioClip _verloren;

        private GameManager _gameManager;

        EnemyBase _enemy;

        // Start is called before the first frame update
        void Start()
        {
            _enemy.OnEnemyDeath += EnemyDeath;
        }

        // Update is called once per frame
        void Update()
        {
            if(_gameManager.IsGameOver)
            {
                _audioSource.clip = _verloren;
                _audioSource.Play();
            }           

        }


        public void EnemyDeath()
        {
            _audioSource.clip = _gewonnen[3];
            _audioSource.Play();
        }





    }
}
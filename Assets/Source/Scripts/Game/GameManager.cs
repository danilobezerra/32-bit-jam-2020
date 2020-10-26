using System;
using UnityEngine;

namespace AncientTech.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsGameRunning { get; private set; }
        public int CurrentLevel
        {
            get => PlayerPrefs.GetInt("Level", 0);
            private set => PlayerPrefs.SetInt("Level", value);
        }

        private int _currentScore = 0;
        private float _currentLife = 1f;
        private float _currentTime = 60;

        private void Awake()
        {
            if (Instance == null) {
                Instance = GetComponent<GameManager>();
            }
        }

        private void Start()
        {
            IsGameRunning = true;
        }
        
        private void Update()
        {
            if (!IsGameRunning) return;

            if (Input.GetButtonDown("Cancel")) {
                if (Time.timeScale > 0f) {
                    //_headUpDisplay.SetActive(false);
                    //_pauseDisplay.SetActive(true);

                    Time.timeScale = 0;
                } else {
                    Time.timeScale = 1f;

                    //_pauseDisplay.SetActive(false);
                    //_headUpDisplay.SetActive(true);
                }
            } else {
                CountdownTime(Time.deltaTime);
            }
        }

        public void GameOverByContent()
        {
            IsGameRunning = false;
            //SceneManager.LoadScene(_endingScene);
        }

        public void ImproveScore(int i)
        {
            //throw new NotImplementedException();
        }

        public void ReplenishLife(float f)
        {
            //throw new NotImplementedException();
        }

        public string LevelComplete()
        {
            //throw new NotImplementedException();
            return "";
        }

        public void DepleteLife(float f)
        {
            //throw new NotImplementedException();
        }
        
        private void CountdownTime(float seconds)
        {
            _currentTime -= seconds;

            if (_currentTime < 1) {
                StopAllCoroutines();
                GameOver();
            }

            //_timeView.CurrentTime = _currentTime;
        }
        
        private void GameOver()
        {
            IsGameRunning = false;

            //PlayerPrefs.SetFloat("Life", _currentLife);
            //PlayerPrefs.SetFloat("Time", _currentTime);
            //PlayerPrefs.SetInt("Score", _currentScore);

            //SceneManager.LoadScene(_endingScene);
        }
    }
}

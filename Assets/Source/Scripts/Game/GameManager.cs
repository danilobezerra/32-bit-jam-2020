using AncientTech.UI;
using UnityEngine;

namespace AncientTech.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GamePlayView gamePlayView;

        public bool IsGameRunning { get; private set; }
        public int CurrentLevel
        {
            get => PlayerPrefs.GetInt("Level", 1);
            private set => PlayerPrefs.SetInt("Level", value);
        }

        private int _currentScore = 0;
        private float _currentLife = 1f;
        private float _currentTime = 60;

        private void Awake()
        {
            Application.targetFrameRate = 24;
            
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
                    gamePlayView.SetPaused(true);

                    Time.timeScale = 0;
                } else {
                    Time.timeScale = 1f;

                    gamePlayView.SetPaused(false);
                }
            } else {
                //CountdownTime(Time.deltaTime);
            }
        }

        public void GameOverByContent()
        {
            IsGameRunning = false;
            //SceneManager.LoadScene(_endingScene);
        }

        public void AddScore(int amount)
        {
            if (_currentScore + amount > 9999) {
                _currentScore = 9999;
            } else {
                _currentScore += amount;
            }

            gamePlayView.SetCurrentScore(_currentScore);
        }

        public void ReplenishLife(float amount)
        {
            if (_currentLife + amount > 1) {
                _currentLife = 1;
            } else {
                _currentLife += amount;
            }

            gamePlayView.SetCurrentLife(_currentLife);
        }

        public string LevelComplete()
        {
            //throw new NotImplementedException();
            return "";
        }

        public void DepleteLife(float amount)
        {
            if (_currentLife - amount < 0.01f) {
                _currentLife = 0;

                StopAllCoroutines();
                GameOver();
            } else {
                _currentLife -= amount;
            }

            gamePlayView.SetCurrentLife(_currentLife);
        }
        
        private void CountdownTime(float seconds)
        {
            _currentTime -= seconds;

            if (_currentTime < 1) {
                StopAllCoroutines();
                GameOver();
            }

            gamePlayView.SetCurrentTime(_currentTime);
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

using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AncientTech.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AncientTech.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GamePlayView gamePlayView;

        public bool IsGameRunning { get; private set; }
        public int CurrentLevel
        {
            get => PlayerPrefs.GetInt("Level", 0);
            private set => PlayerPrefs.SetInt("Level", value);
        }

        private int _currentScore = 0;
        private float _currentLife = 1f;

        private void Awake()
        {
            Application.targetFrameRate = 24;
            
            if (Instance == null) {
                Instance = GetComponent<GameManager>();
            }
        }

        private IEnumerator Start()
        {
            gamePlayView.SetLoading(true);

            _currentLife = PlayerPrefs.GetFloat("Life", 1f);
            _currentScore = PlayerPrefs.GetInt("Score", 0);
            
            yield return new WaitForSeconds(3f);
            
            gamePlayView.SetCurrentLife(_currentLife);
            gamePlayView.SetCurrentScore(_currentScore);
        
            //PlayerPrefs.DeleteAll(); // On Play button press
            
            //var backgroundMusic = Camera.main.GetComponent<AudioSource>();
            //backgroundMusic.Play();
            
            gamePlayView.SetLoading(false);
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
            }
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

        public void LevelComplete()
        {
            IsGameRunning = false;
            
            PlayerPrefs.SetFloat("Life", _currentLife);
            PlayerPrefs.SetInt("Score", _currentScore);
            
            var path = Path.Combine(Application.streamingAssetsPath, "Levels/");
            var info = new DirectoryInfo(path);

            var levels = info.GetFiles()
                .Where(file => file.Extension == ".txt")
                .Select(txt => Regex.Match(txt.Name, @"\d+").Value)
                .Select(int.Parse)
                .ToArray();
            
            if (CurrentLevel + 1 > levels[levels.Length - 1]) {
                CurrentLevel = 0;
            } else {
                CurrentLevel++;
            }
            
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name);
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
        
        private void GameOver()
        {
            IsGameRunning = false;

            PlayerPrefs.SetFloat("Life", _currentLife);
            PlayerPrefs.SetInt("Score", _currentScore);

            //SceneManager.LoadScene(_endingScene);
        }
    }
}

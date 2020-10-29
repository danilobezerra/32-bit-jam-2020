using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AncientTech.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName;
        [SerializeField] private EventSystem eventSystem;
        
        [Header("Text View")]
        
        [SerializeField] private Text highScoreTextView;
        [SerializeField] private Text scoreTextView;

        [Header("Buttons")]
        
        [SerializeField] private ButtonView continueButton;
        [SerializeField] private ButtonView exitButton;

        private void Start()
        {
            continueButton.Setup("Continue", ContinueGame);
            exitButton.Setup("Exit", ExitGame);
        }

        private void ContinueGame()
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name);
        }

        private void ExitGame()
        {
            Debug.Log("Give up!");
            
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.DeleteKey("Level");
            PlayerPrefs.DeleteKey("Life");
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(mainMenuSceneName);
        }

        private void NewRecord()
        {
            var color = highScoreTextView.color;
            color.a = color.a > 0 ? 1f : 0;
            highScoreTextView.color = color;
        }

        public void SetActive(bool isActive, int score = 0)
        {
            gameObject.SetActive(isActive);
            if (!isActive) return;
            
            eventSystem.SetSelectedGameObject(continueButton.gameObject);
            
            var highScore = PlayerPrefs.GetInt("High Score", 0);

            if (score > highScore) {
                highScoreTextView.text = $"High Score: {score:D4}";
                PlayerPrefs.SetInt("High Score", score);
                PlayerPrefs.Save();
                
                highScoreTextView.color = new Color(1f, .4f, 0, 1f); //255, 110, 0
            } else {
                highScoreTextView.text = $"High Score: {highScore:D4}";
            }
            
            scoreTextView.text = $"Score: {score:D4}";
        }
    }
}

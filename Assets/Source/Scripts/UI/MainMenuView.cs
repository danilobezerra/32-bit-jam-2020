using AncientTech.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AncientTech.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private string gameSceneName;
        
        [Header("Text View")]
        
        [SerializeField] private Text versionTextView;
        [SerializeField] private Text companyNameTextView;

        [Header("Buttons")]
        
        [SerializeField] private ButtonView startButton;
        [SerializeField] private ButtonView continueButton;
        [SerializeField] private ButtonView quitButton;

        private void Start()
        {
            Time.timeScale = 1f;
            
            versionTextView.text = $"v{Application.version}";
            companyNameTextView.text = $"32-bit Jam 2020\n\xA9 {Application.companyName}";

            startButton.Setup("New Game", StartGame);
            continueButton.Setup("Load Game", ContinueGame);

#if UNITY_STANDALONE
            quitButton.Setup("Quit", Quit);
#else
            quitButton.SetActive(false);
#endif
            
            AudioManager.Instance.PlayLevelMusic();
        }

        private void StartGame()
        {
            Debug.Log("New Game");
            
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.DeleteKey("Level");
            PlayerPrefs.DeleteKey("Life");
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(gameSceneName);
            AudioManager.Instance.FadeDown(1.5f);
        }
        
        private void ContinueGame()
        {
            Debug.Log("Load Game");

            if (PlayerPrefs.HasKey("Level")
                && PlayerPrefs.HasKey("Life")
                && PlayerPrefs.HasKey("Score")) {
                // TODO: Show message "Save data not found"
            }
            
            SceneManager.LoadScene(gameSceneName);
            AudioManager.Instance.FadeDown(1.5f);
        }
        
        private static void Quit()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}

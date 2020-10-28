using System;
using UnityEngine;
using UnityEngine.UI;

namespace AncientTech.UI
{
    public class GamePlayView : MonoBehaviour
    {
        [SerializeField] private Text lifeTextView;
        [SerializeField] private Text scoreTextView;
        
        [SerializeField] private GameObject pauseView;
        [SerializeField] private GameObject loadingView;

        public void SetPaused(bool isPaused)
        {
            lifeTextView.gameObject.SetActive(!isPaused);
            scoreTextView.gameObject.SetActive(!isPaused);
            
            pauseView.SetActive(isPaused);
        }
        
        public void SetLoading(bool isLoading)
        {
            lifeTextView.gameObject.SetActive(!isLoading);
            scoreTextView.gameObject.SetActive(!isLoading);
            
            loadingView.SetActive(isLoading);
        }

        public void SetCurrentLife(float life)
        {
            lifeTextView.text = $"Fire: {life:P0}";
        }
        
        public void SetCurrentScore(int score)
        {
            scoreTextView.text = $"Gold: {score:D4}";
        }
    }
}

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

        public void SetPaused(bool isPaused)
        {
            lifeTextView.gameObject.SetActive(!isPaused);
            scoreTextView.gameObject.SetActive(!isPaused);
            
            pauseView.SetActive(isPaused);
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

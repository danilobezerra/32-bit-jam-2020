using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace AncientTech.Game
{
    public class AudioManager : MonoBehaviour
    {
        private const float ResetTime = 0.01f;
        
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource mainSource;
        [SerializeField] private AudioMixer mainMixer;

        [Header("Mixer Snapshots")]

        [SerializeField] private AudioMixerSnapshot volumeDown;
        [SerializeField] private AudioMixerSnapshot volumeUp;
        
        [Header("Tracks")]
        
        [SerializeField] private AudioClip mainMenuTrack;
        [SerializeField] private AudioClip gameplayTrack;
        
        private void Awake()
        {
            if (Instance == null) {
                Instance = GetComponent<AudioManager>();
            } else {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
        
        public void PlayLevelMusic()
        {
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            var currentTrack = mainSource.clip;
            
            switch (sceneIndex) {
                case 0:
                    mainSource.clip = mainMenuTrack;
                    break;
                case 1:
                    mainSource.clip = gameplayTrack;
                    break;
            }

            if (mainSource.clip == currentTrack) {
                return;
            }
            
            FadeUp(ResetTime);
            mainSource.Play();
        }

        public void PlaySelectedMusic(AudioClip clip)
        {
            mainSource.clip = clip;
            mainSource.Play();
        }

        public void FadeUp(float time)
        {
            volumeUp.TransitionTo(time);
        }

        public void FadeDown(float time)
        {
            volumeDown.TransitionTo(time);
        }
    }
}

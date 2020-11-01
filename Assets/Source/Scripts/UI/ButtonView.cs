using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AncientTech.UI
{
    public class ButtonView : MonoBehaviour, ISelectHandler
    {
        [SerializeField] private Text label;
        [SerializeField] private AudioClip selectSfx;

        private AudioSource _audio;
        private Action _callback;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void Setup(string labelText, Action buttonCallback)
        {
            label.text = labelText;
            _callback = buttonCallback;
        }
    
        public void OnClick()
        {
            _callback?.Invoke();
        }

        public void SetActive(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _audio.PlayOneShot(selectSfx);
        }
    }
}

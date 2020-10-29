using System;
using UnityEngine;
using UnityEngine.UI;

namespace AncientTech.UI
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Text label;
        
        private Action _callback;

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
    }
}

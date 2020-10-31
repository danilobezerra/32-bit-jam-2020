using System;
using UnityEngine;

namespace AncientTech.Game
{
    public class Health : MonoBehaviour
    {
        [Range(0, 1f)]
        [SerializeField] private float points = .1f;
        [SerializeField] private AudioClip pickupClip;
        
        public float Points => points;
        public AudioClip PickupClip => pickupClip;

        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _audio.Play();
        }
    }
}

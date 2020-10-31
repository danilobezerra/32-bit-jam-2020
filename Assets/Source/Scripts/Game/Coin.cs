using UnityEngine;

namespace AncientTech.Game
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int value = 100;
        [SerializeField] private AudioClip pickupClip;
        
        public int Value => value;
        public AudioClip PickupClip => pickupClip;
    }
}

using UnityEngine;

namespace AncientTech.Game
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int value = 100;
        public int Value => value;
    }
}

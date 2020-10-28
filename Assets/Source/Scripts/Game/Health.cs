using UnityEngine;

namespace AncientTech.Game
{
    public class Health : MonoBehaviour
    {
        [Range(0, 1f)]
        [SerializeField] private float points = .1f;
        public float Points => points;
    }
}

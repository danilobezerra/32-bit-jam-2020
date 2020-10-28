using UnityEngine;

namespace AncientTech.Game
{
    public class RotateAround : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;

        private void Update()
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * speed));
        }
    }
}

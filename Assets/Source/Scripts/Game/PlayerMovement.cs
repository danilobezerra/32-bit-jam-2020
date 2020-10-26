using System;
using System.Collections;
using UnityEngine;

namespace AncientTech.Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private string tagWall = "Wall";
        [SerializeField] private string tagGoal = "Goal";
        [SerializeField] private string tagCoin = "Coin";
        [SerializeField] private string tagHealth = "Health";

        [SerializeField] private AudioClip[] _sounds;

        [SerializeField] private float moveFactor = 1f;
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float gridSize = 1f;

        private AudioSource _audioSource;

        public bool IsMoving { get; private set; }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.IsGameRunning) return;
            if (IsMoving) return;

            var horizontal = (int) Input.GetAxisRaw("Horizontal");
            var vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0) {
                vertical = 0;
            }

            if (horizontal == 0 && vertical == 0) return;
            var input = new Vector3(horizontal, 0, vertical);

            if (!CanMove(input)) return;
            StartCoroutine(Move(input));
        }

        private IEnumerator Move(Vector3 input)
        {
            IsMoving = true;
            //_audioSource.PlayOneShot(_sounds[0]);

            if (GameManager.Instance.IsGameRunning) {
                GameManager.Instance.DepleteLife(0.05f);
            }

            var startPosition = transform.position;
            float time = 0;

            var endPosition = new Vector3(startPosition.x + Math.Sign(input.x) * gridSize,
                startPosition.y, startPosition.z + Math.Sign(input.z) * gridSize);

            while (time < 1f) {
                time += Time.deltaTime * (moveSpeed / gridSize) * moveFactor;
                transform.position = Vector3.Lerp(startPosition, endPosition, time);
                yield return null;
            }

            IsMoving = false;
            yield return 0;
        }

        private bool CanMove(Vector3 direction)
        {
            if (!Physics.Raycast(transform.position, direction, out var hit, 1f)) {
                return true;
            }

            if (hit.collider.CompareTag(tagWall)) {
                // TODO: Play wall bump sound
                //GetComponent<AudioSource>().PlayOneShot(_sounds[1]);
                return false;
            }

            if (hit.collider.CompareTag(tagGoal)) {
                if (GameManager.Instance.IsGameRunning) {
                    //_audioSource.PlayOneShot(_sounds[2]);
                    //StartCoroutine(GameManager.Instance.LevelComplete());
                }

                return true;
            }

            if (hit.collider.CompareTag(tagCoin)) {
                if (GameManager.Instance.IsGameRunning) {
                    //_audioSource.PlayOneShot(_sounds[3]);
                    //GameManager.Instance.ImproveScore(100);
                    // TODO: Play coin caught sound

                    Destroy(hit.collider.gameObject);
                }

                return true;
            }

            if (hit.collider.CompareTag(tagHealth)) {
                if (GameManager.Instance.IsGameRunning) {
                    //_audioSource.PlayOneShot(_sounds[4]);
                    //GameManager.Instance.ReplenishLife(0.1f);
                    // TODO: Play cure sound

                    Destroy(hit.collider.gameObject);
                }

                return true;
            }

            return true;
        }
    }
}

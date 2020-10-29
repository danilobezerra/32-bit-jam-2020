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

        [SerializeField] private Animator animatorController;
        private AudioSource _audioSource;
        
        private static readonly int IsMovingAnimatorProperty = Animator.StringToHash("Is Moving");
        public bool IsMoving
        {
            get => animatorController.GetBool(IsMovingAnimatorProperty);
            private set => animatorController.SetBool(IsMovingAnimatorProperty, value);
        }

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

            RotateCharacter(input);

            if (!CanMove(input)) return;
            StartCoroutine(Move(input));
        }

        private void RotateCharacter(Vector3 input)
        {
            if (input == Vector3.forward) {
                animatorController.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            
            if (input == Vector3.left) {
                animatorController.transform.eulerAngles = new Vector3(0, -90, 0);
            }
            
            if (input == Vector3.back) {
                animatorController.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            
            if (input == Vector3.right) {
                animatorController.transform.eulerAngles = new Vector3(0, 90, 0);
            }
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
            
            Debug.DrawRay(transform.position, direction, Color.magenta, .5f);

            if (hit.collider.CompareTag(tagWall)) {
                // TODO: Play wall bump sound
                //GetComponent<AudioSource>().PlayOneShot(_sounds[1]);
                return false;
            }

            if (hit.collider.CompareTag(tagGoal)) {
                if (GameManager.Instance.IsGameRunning) {
                    //_audioSource.PlayOneShot(_sounds[2]);
                    GameManager.Instance.LevelComplete();
                }

                return true;
            }

            if (hit.collider.CompareTag(tagCoin)) {
                if (GameManager.Instance.IsGameRunning) {
                    //_audioSource.PlayOneShot(_sounds[3]);
                    var coin = hit.collider.gameObject.GetComponent<Coin>();
                    GameManager.Instance.AddScore(coin.Value); // TODO: object's properties 
                    // TODO: Play coin caught sound

                    Destroy(hit.collider.gameObject);
                }

                return true;
            }

            if (hit.collider.CompareTag(tagHealth)) {
                if (GameManager.Instance.IsGameRunning) {
                    //_audioSource.PlayOneShot(_sounds[4]);
                    var health = hit.collider.gameObject.GetComponent<Health>();
                    GameManager.Instance.ReplenishLife(health.Points);
                    // TODO: Play cure sound

                    Destroy(hit.collider.gameObject);
                }

                return true;
            }

            return true;
        }
    }
}

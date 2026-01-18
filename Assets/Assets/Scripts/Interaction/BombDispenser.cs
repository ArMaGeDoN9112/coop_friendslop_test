using System.Collections;
using Mirror;
using UnityEngine;

namespace Coop.Interaction
{
    public class BombDispenser : NetworkBehaviour, IInteractable
    {
        [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnDelay = 1.0f;

        // Звуки и эффекты
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _activationParticles;

        private bool _isBusy;

        public string InteractionPrompt => "Press E to get Bomb";

        public void OnInteract() => ActivateDispenser();

        // Вызывается из PlayerInteraction через Command
        [Server]
        private void ActivateDispenser()
        {
            if (_isBusy) return;

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            _isBusy = true;

            RpcPlayEffects();

            yield return new WaitForSeconds(_spawnDelay);

            var bomb = Instantiate(_bombPrefab, _spawnPoint.position, _spawnPoint.rotation);
            NetworkServer.Spawn(bomb);

            var rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(_spawnPoint.forward * 5f + Vector3.up * 2f, ForceMode.Impulse);

            _isBusy = false;
        }

        [ClientRpc]
        private void RpcPlayEffects()
        {
            if (_audioSource) _audioSource.Play();
            if (_activationParticles) _activationParticles.Play();
        }
    }
}
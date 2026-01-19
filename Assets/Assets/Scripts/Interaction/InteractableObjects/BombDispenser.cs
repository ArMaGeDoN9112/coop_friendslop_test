using System.Collections;
using Mirror;
using UnityEngine;

namespace Coop.Interaction.InteractableObjects
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class BombDispenser : NetworkBehaviour, IInteractable
    {
        [Header("Settings")] [SerializeField] private float _spawnDelay = 1.0f;
        [SerializeField] private string _promptText = "Press E to Dispense";

        [Header("Visuals")] [SerializeField]
        private Transform _interactionAnchor; // Сюда перетяни пустой объект над автоматом

        [Header("Spawning")] [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private Transform _spawnPoint;

        [Header("Effects")] [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _activationParticles;

        [SyncVar] private bool _isBusy;

        #region IInteractable

        public string InteractionPrompt => _isBusy ? "Dispensing..." : _promptText;

        public bool CanInteract => !_isBusy;

        public Transform InteractionAnchor => _interactionAnchor != null ? _interactionAnchor : transform;

        public void OnInteract(GameObject interactor) { ActivateDispenser(); }

        #endregion


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

            if (bomb.TryGetComponent(out Rigidbody rb))
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
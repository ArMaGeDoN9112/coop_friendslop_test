using System.Collections;
using Coop.Configs;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Interaction.InteractableObjects
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class BombDispenser : NetworkBehaviour, IInteractable
    {
        [SerializeField] private string _promptText;
        [SerializeField] private Transform _interactionAnchor;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ParticleSystem _activationParticles;

        [SyncVar] private bool _isBusy;

        private AudioSource _audioSource;
        private GameObject _bombPrefab;
        private DiContainer _container;
        private BombDispencerConfig _bombDispencerConfig;

        [Inject]
        public void Construct(DiContainer container, BombConfig bombConfig, BombDispencerConfig bombDispencerConfig)
        {
            _container = container;
            _bombPrefab = bombConfig.BombPrefab;
            _bombDispencerConfig = bombDispencerConfig;
        }

        private void Awake() => _audioSource = GetComponent<AudioSource>();

        #region IInteractable

        public string InteractionPrompt => _bombDispencerConfig.InteractionHintText;

        public bool CanInteract => !_isBusy;

        public Transform InteractionAnchor => _interactionAnchor;

        public void OnInteract(GameObject interactor) => ActivateDispenser();

        #endregion


        [Server]
        private void ActivateDispenser()
        {
            if (_isBusy) return;

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine() //TODO: refactor with unitask
        {
            _isBusy = true;

            RpcPlayEffects();

            yield return new WaitForSeconds(_bombDispencerConfig.SpawnDelay);

            var bomb = _container.InstantiatePrefab(_bombPrefab, _spawnPoint.position, _spawnPoint.rotation, null);
            NetworkServer.Spawn(bomb);

            if (bomb.TryGetComponent(out Rigidbody rb))
                rb.AddForce(_spawnPoint.forward * _bombDispencerConfig.ForwardStrength +
                            Vector3.up * _bombDispencerConfig.UpStrength, ForceMode.Impulse);

            _isBusy = false;
        }

        [ClientRpc]
        private void RpcPlayEffects()
        {
            _audioSource.clip = _bombDispencerConfig.UsageSound;
            _audioSource.Play();

            if (_activationParticles) _activationParticles.Play();
        }
    }
}
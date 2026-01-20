using System.Collections;
using Coop.Configs;
using Coop.Utils;
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
        private BombDispencerConfig _bombDispencerConfig;
        private IPrefabFactory _prefabFactory;

        [Inject]
        public void Construct(BombConfig bombConfig, BombDispencerConfig bombDispencerConfig,
            IPrefabFactory prefabFactory)
        {
            _bombPrefab = bombConfig.BombPrefab;
            _bombDispencerConfig = bombDispencerConfig;
            _prefabFactory = prefabFactory;
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

            var bomb = _prefabFactory.Create(_bombPrefab, null, _spawnPoint.position, _spawnPoint.rotation);
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
using System.Collections;
using Coop.Configs;
using Coop.Player.Components;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Combat
{
    public class Bomb : NetworkBehaviour
    {
        private Renderer _renderer;
        private Collider _collider;
        private BombConfig _bombConfig;

        [Inject]
        private void Construct(BombConfig bombConfig) => _bombConfig = bombConfig;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
        }

        private void Start() => Debug.Log(_bombConfig);

        public override void OnStartServer() => StartCoroutine(ExplosionRoutine());

        private IEnumerator ExplosionRoutine()
        {
            yield return new WaitForSeconds(_bombConfig.TimeToExplode);
            Explode();
        }

        [Server]
        private void Explode()
        {
            RpcHideVisuals();
            RpcSpawnExplosionEffect(transform.position);

            var hits = Physics.OverlapSphere(transform.position, _bombConfig.ExplosionRadius);
            foreach (var hit in hits)
            {
                var health = hit.GetComponent<PlayerHealth>() ?? hit.GetComponentInParent<PlayerHealth>();
                if (health) health.TakeDamage();
            }

            StartCoroutine(DestroyWithDelay());
        }

        [ClientRpc]
        private void RpcHideVisuals()
        {
            _renderer.enabled = false;
            _collider.enabled = false;
        }

        [ClientRpc]
        private void RpcSpawnExplosionEffect(Vector3 position)
        {
            var effect = Instantiate(_bombConfig.ExplosionVFXPrefab, position, Quaternion.identity);
            var audioSource = effect.GetComponent<AudioSource>();
            audioSource.clip = _bombConfig.SoundEffect;
            audioSource.Play();
        }

        private IEnumerator DestroyWithDelay()
        {
            yield return new WaitForSeconds(0.1f);
            NetworkServer.Destroy(gameObject);
        }
    }
}
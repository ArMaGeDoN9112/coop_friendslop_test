using Coop.Configs;
using Coop.Player.Components;
using Cysharp.Threading.Tasks;
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

        public override void OnStartServer() => ExplosionRoutineAsync().Forget();

        private async UniTaskVoid ExplosionRoutineAsync()
        {
            await UniTask.WaitForSeconds(_bombConfig.TimeToExplode,
                cancellationToken: this.GetCancellationTokenOnDestroy());

            if (!this) return;
            Explode();
        }

        [Server]
        private void Explode()
        {
            RpcSpawnExplosionEffect(transform.position);

            var hits = Physics.OverlapSphere(transform.position, _bombConfig.ExplosionRadius);
            foreach (var hit in hits)
            {
                var health = hit.GetComponent<PlayerHealth>() ?? hit.GetComponentInParent<PlayerHealth>();
                if (health) health.TakeDamage();
            }

            DestroyWithDelayAsync().Forget();
        }

        [ClientRpc]
        private void RpcSpawnExplosionEffect(Vector3 position)
        {
            var effect = Instantiate(_bombConfig.ExplosionVFXPrefab, position, Quaternion.identity);
            var audioSource = effect.GetComponent<AudioSource>();
            audioSource.clip = _bombConfig.SoundEffect;
            audioSource.Play();
        }

        private async UniTaskVoid DestroyWithDelayAsync()
        {
            await UniTask.WaitForSeconds(0.1f,
                cancellationToken: this.GetCancellationTokenOnDestroy());

            NetworkServer.Destroy(gameObject);
        }
    }
}
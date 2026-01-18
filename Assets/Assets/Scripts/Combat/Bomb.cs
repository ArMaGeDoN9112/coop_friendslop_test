using System.Collections;
using Coop.Player;
using Mirror;
using UnityEngine;

namespace Coop.Combat
{
    public class Bomb : NetworkBehaviour
    {
        [SerializeField] private float _timeToExplode = 3f;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private GameObject _explosionVFXPrefab;

        public override void OnStartServer() => StartCoroutine(ExplosionRoutine());

        private IEnumerator ExplosionRoutine()
        {
            yield return new WaitForSeconds(_timeToExplode);
            Explode();
        }

        [Server]
        private void Explode()
        {
            RpcExplosionEffects(transform.position);

            var hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (var hit in hits)
            {
                var health = hit.GetComponent<PlayerHealth>() ?? hit.GetComponentInParent<PlayerHealth>();
                if (health) health.TakeDamage();
            }

            NetworkServer.Destroy(gameObject);
        }

        [ClientRpc]
        private void RpcExplosionEffects(Vector3 position)
        {
            if (_explosionVFXPrefab) Instantiate(_explosionVFXPrefab, position, Quaternion.identity);
            // Тут можно добавить звук взрыва
        }
    }
}
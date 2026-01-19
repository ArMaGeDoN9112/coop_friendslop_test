using Coop.Configs;
using Coop.Interaction;
using Mirror;
using UnityEngine;

namespace Coop.Player.Services
{
    public class PlayerInteractionService : IPlayerInteractionService
    {
        private readonly Camera _camera;
        private readonly NetworkIdentity _localIdentity;
        private readonly LayerMask _interactLayer;
        private readonly PlayerConfig _playerConfig;

        public PlayerInteractionService(Camera camera, NetworkIdentity localIdentity, PlayerConfig playerConfig)
        {
            _camera = camera;
            _localIdentity = localIdentity;
            _playerConfig = playerConfig;

            _interactLayer = LayerMask.GetMask("Interactable");
        }

        public bool TryGetInteractable(bool isOnlineInteraction, out IInteractable interactable)
        {
            interactable = null;

            var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out var hit, _playerConfig.InteractDistance, _interactLayer))
            {
                var candidate = hit.collider;
                if (isOnlineInteraction && candidate.GetComponentInParent<NetworkIdentity>() == _localIdentity)
                    return false;

                interactable = hit.collider.GetComponent<IInteractable>() ??
                               hit.collider.GetComponentInParent<IInteractable>();
            }


            return interactable != null;
        }
    }
}
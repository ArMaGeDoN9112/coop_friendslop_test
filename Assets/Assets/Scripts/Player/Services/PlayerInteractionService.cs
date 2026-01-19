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
        private const float InteractDistance = 10f;

        public PlayerInteractionService(Camera camera, NetworkIdentity localIdentity)
        {
            _camera = camera;
            _localIdentity = localIdentity;

            _interactLayer = LayerMask.GetMask("Interactable");
        }

        public bool TryGetInteractable(bool isOnlineInteraction, out IInteractable interactable)
        {
            interactable = null;

            var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out var hit, InteractDistance, _interactLayer))
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
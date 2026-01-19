using Coop.Interaction;
using Mirror;
using UnityEngine;

namespace Coop.Player.Services
{
    public class PlayerInteractionService : IPlayerInteractionService
    {
        private readonly LayerMask _interactLayer;
        private readonly Camera _camera;
        private const float InteractDistance = 10f;
        private readonly NetworkIdentity _localIdentity;

        public PlayerInteractionService(Camera camera, NetworkIdentity localIdentity)
        {
            _camera = camera;
            _localIdentity = localIdentity;

            _interactLayer = LayerMask.GetMask("Interactable");
        }

        public bool TryGetInteractable(out IInteractable interactable)
        {
            interactable = null;

            var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.red);

            if (Physics.Raycast(ray, out var hit, InteractDistance, _interactLayer))
            {
                var candidate = hit.collider;
                if (candidate.GetComponentInParent<NetworkIdentity>() == _localIdentity) return false;

                interactable = hit.collider.GetComponent<IInteractable>() ??
                               hit.collider.GetComponentInParent<IInteractable>();
            }


            return interactable != null;
        }
    }
}
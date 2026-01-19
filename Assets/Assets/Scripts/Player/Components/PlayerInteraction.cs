using Coop.Interaction;
using Coop.Player.Services;
using Mirror;
using UnityEngine;
using Zenject;

// Твой неймспейс с PlayerInteractionService

namespace Coop.Player.Components
{
    public class PlayerInteraction : NetworkBehaviour
    {
        private IPlayerInteractionService _interactionService;
        private IInteractionUIService _uiService;

        [Inject]
        public void Construct(IPlayerInteractionService interactionService, IInteractionUIService uiService)
        {
            _interactionService = interactionService;
            _uiService = uiService;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            bool found = _interactionService.TryGetInteractable(out var interactable);

            if (found && interactable.CanInteract)
                _uiService.ShowHint(interactable);
            else
                _uiService.HideHint();

            if (!found || !Input.GetKeyDown(KeyCode.E)) return;

            if (interactable.CanInteract) HandleInteraction(interactable);
        }

        private void OnDisable()
        {
            if (isLocalPlayer && _uiService != null) _uiService.HideHint();
        }

        private void HandleInteraction(IInteractable interactable)
        {
            var component = interactable as Component;
            if (!component) return;

            var netIdentity = component.GetComponentInParent<NetworkIdentity>();
            if (netIdentity) CmdTryInteract(netIdentity.gameObject);
        }

        [Command]
        private void CmdTryInteract(GameObject targetObj)
        {
            if (!targetObj) return;

            var interactable = targetObj.GetComponent<IInteractable>() ??
                               targetObj.GetComponentInParent<IInteractable>();

            if (interactable is { CanInteract: true }) interactable.OnInteract(gameObject);
        }
    }
}
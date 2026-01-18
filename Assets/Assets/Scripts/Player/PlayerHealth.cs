using Coop.Interaction;
using Mirror;
using UnityEngine;

namespace Coop.Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnStateChanged))]
        public bool isWounded;

        private CharacterController _characterController;
        private ReviveComponent _reviveComponent;
        private PlayerMovement _movement;

        private void Awake() => _characterController = GetComponent<CharacterController>();

        // Вызывается бомбой (Server only)
        public void TakeDamage()
        {
            if (isWounded) return;

            isWounded = true;
            Debug.Log($"Player {netId} is wounded!");
        }

        public void Revive()
        {
            if (!isWounded) return;

            isWounded = false;
            Debug.Log($"Player {netId} revived!");
        }

        private void OnStateChanged(bool oldState, bool newState)
        {
            if (_reviveComponent) _reviveComponent.ToggleInteractable(newState);
            if (_movement) _movement.SetWoundedState(newState);
            _characterController.height = newState ? 1.5f : 3.0f;
        }
    }
}
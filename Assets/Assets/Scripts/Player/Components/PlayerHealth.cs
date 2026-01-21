using Coop.Interaction.InteractableObjects;
using Mirror;

namespace Coop.Player.Components
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnStateChanged))]
        public bool IsWounded;

        private PlayerMovement _playerMovement;
        private ReviveComponent _reviveComponent;
        private PlayerAnimation _playerAnimation;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _reviveComponent = GetComponent<ReviveComponent>();
            _playerAnimation = GetComponent<PlayerAnimation>();
        }

        [Server]
        public void TakeDamage()
        {
            if (IsWounded) return;

            IsWounded = true;
        }

        [Server]
        public void Revive()
        {
            if (!IsWounded) return;

            IsWounded = false;
        }

        private void OnStateChanged(bool _, bool newState)
        {
            _playerMovement.OnWoundedStateChanged(newState);
            _reviveComponent.ToggleInteractable(newState);
            _playerAnimation.HandleWoundedState(newState);
        }
    }
}
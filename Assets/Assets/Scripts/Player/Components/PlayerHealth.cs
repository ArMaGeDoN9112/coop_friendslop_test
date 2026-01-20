using Coop.Interaction.InteractableObjects;
using Mirror;
using Zenject;

namespace Coop.Player.Components
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnStateChanged))]
        public bool IsWounded;

        private PlayerMovement _movementView;
        private ReviveComponent _reviveComponent;
        private PlayerAnimation _playerAnimation;


        [Inject]
        public void Construct(PlayerMovement movementView) => _movementView = movementView;

        private void Awake()
        {
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
            _movementView.OnWoundedStateChanged(newState);
            _reviveComponent.ToggleInteractable(newState);
            _playerAnimation.HandleWoundedState(newState);
        }
    }
}
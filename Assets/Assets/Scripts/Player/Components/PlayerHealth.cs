using Coop.Interaction.InteractableObjects;
using Coop.Player.Services;
using Mirror;
using Zenject;

namespace Coop.Player.Components
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnStateChanged))]
        public bool isWounded;

        private IPlayerHealthService _healthService;
        private PlayerMovement _movementView;
        private ReviveComponent _reviveComponent;
        private PlayerAnimation _playerAnimation;


        [Inject]
        public void Construct(IPlayerHealthService healthService, PlayerMovement movementView)
        {
            _healthService = healthService;
            _movementView = movementView;
        }

        private void Awake()
        {
            _reviveComponent = GetComponent<ReviveComponent>();
            _playerAnimation = GetComponent<PlayerAnimation>();
        }

        [Server]
        public void TakeDamage()
        {
            if (isWounded) return;

            isWounded = true;
        }

        [Server]
        public void Revive()
        {
            if (!isWounded) return;

            isWounded = false;
        }

        private void OnStateChanged(bool _, bool newState)
        {
            _healthService.ApplyStateEffects(newState);
            _movementView.OnWoundedStateChanged(newState);
            _reviveComponent.ToggleInteractable(newState);
            _playerAnimation.HandleWoundedState(newState);
        }
    }
}
using Coop.Configs;
using Coop.Player.Components;
using UnityEngine;
using Zenject;

namespace Coop.Interaction.InteractableObjects
{
    public class ReviveComponent : MonoBehaviour, IInteractable
    {
        [Tooltip("Точка, где будет висеть текст (обычно над головой)"), SerializeField]
        private Transform _interactionAnchor;

        private PlayerHealth _playerHealth;

        [Inject]
        private void Construct(PlayerConfig playerConfig) => InteractionPrompt = playerConfig.ReviveHint;

        private void Awake() => _playerHealth = GetComponent<PlayerHealth>();

        public void ToggleInteractable(bool isWounded) => CanInteract = isWounded;

        #region IInteractable

        public string InteractionPrompt { get; private set; }

        public bool CanInteract { get; private set; }

        public Transform InteractionAnchor => _interactionAnchor;

        public void OnInteract(GameObject interactor) => _playerHealth.Revive();

        #endregion
    }
}
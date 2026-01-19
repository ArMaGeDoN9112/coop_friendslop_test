using Coop.Player.Components;
using UnityEngine;

namespace Coop.Interaction.InteractableObjects
{
    public class ReviveComponent : MonoBehaviour, IInteractable
    {
        [Header("UI Settings")] [SerializeField]
        private string _promptText = "Hold E to Revive";

        [Tooltip("Точка, где будет висеть текст (обычно над головой)")] [SerializeField]
        private Transform _interactionAnchor;

        private PlayerHealth _playerHealth;


        private void Awake() => _playerHealth = GetComponent<PlayerHealth>();

        public void ToggleInteractable(bool isWounded) => CanInteract = isWounded;

        #region IInteractable

        public string InteractionPrompt => _promptText;

        public bool CanInteract { get; private set; }

        public Transform InteractionAnchor => _interactionAnchor;

        public void OnInteract(GameObject interactor)
        {
            if (!_playerHealth) return;

            _playerHealth.Revive();
        }

        #endregion
    }
}
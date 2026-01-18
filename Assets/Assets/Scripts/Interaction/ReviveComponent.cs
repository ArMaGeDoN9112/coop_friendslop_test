using Coop.Player;
using UnityEngine;

namespace Coop.Interaction
{
    public class ReviveComponent : MonoBehaviour, IInteractable
    {
        private PlayerHealth _playerHealth;
        private bool _canBeRevived;

        public string InteractionPrompt => "Press E to Revive";

        private void Awake() => _playerHealth = GetComponent<PlayerHealth>();

        public void ToggleInteractable(bool active) => _canBeRevived = active;

        public void OnInteract(GameObject interactor) => _playerHealth.Revive();
    }
}
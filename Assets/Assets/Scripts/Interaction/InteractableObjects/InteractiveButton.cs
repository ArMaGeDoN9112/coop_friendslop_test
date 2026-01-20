using System;
using Mirror;
using UnityEngine;

namespace Coop.Interaction.InteractableObjects
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class InteractiveButton : MonoBehaviour, IInteractable
    {
        private Action _onPressAction;

        public void Setup(Action onPressAction) => _onPressAction = onPressAction;
        private void OnDestroy() => _onPressAction = null;


        #region IInteractable

        [field: SerializeField] public string InteractionPrompt { get; private set; }
        [field: SerializeField] public Transform InteractionAnchor { get; private set; }
        public bool CanInteract => true;
        public void OnInteract(GameObject interactor) => _onPressAction?.Invoke();

        #endregion
    }
}
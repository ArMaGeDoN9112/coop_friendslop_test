using UnityEngine;

namespace Coop.Interaction
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }

        bool CanInteract { get; }

        Transform InteractionAnchor { get; }

        void OnInteract(GameObject interactor);
    }
}
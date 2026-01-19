using Coop.Interaction;

namespace Coop.Player.Services
{
    public interface IPlayerInteractionService
    {
        public bool TryGetInteractable(bool isOnlineInteraction, out IInteractable interactable);
    }
}
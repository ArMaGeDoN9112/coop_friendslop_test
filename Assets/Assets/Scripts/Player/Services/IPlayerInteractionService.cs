using Coop.Interaction;

namespace Coop.Player.Services
{
    public interface IPlayerInteractionService
    {
        bool TryGetInteractable(out IInteractable interactable);
    }
}
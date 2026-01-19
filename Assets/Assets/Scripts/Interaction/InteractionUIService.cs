using Coop.Interaction.UI;

namespace Coop.Interaction
{
    public class InteractionUIService : IInteractionUIService
    {
        private readonly InteractionHintView _hintView;

        public InteractionUIService(InteractionHintView hintView) => _hintView = hintView;

        public void ShowHint(IInteractable interactable)
        {
            if (interactable is not { CanInteract: true })
            {
                HideHint();
                return;
            }

            _hintView.Show(interactable.InteractionPrompt, interactable.InteractionAnchor);
        }

        public void HideHint() => _hintView.Hide();
    }
}
namespace Coop.Interaction
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }

        void OnInteract();
    }
}
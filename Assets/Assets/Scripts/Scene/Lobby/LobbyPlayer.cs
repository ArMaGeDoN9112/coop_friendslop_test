using Coop.Interaction;
using Coop.Player.Services;
using UnityEngine;
using Zenject;

namespace Coop.Scene.Lobby
{
    [RequireComponent(typeof(CharacterController))]
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;

        private IPlayerMovementService _movementService;
        private IPlayerInteractionService _interactionService;
        private IInteractionUIService _uiService;

        [Inject]
        public void Construct(IPlayerMovementService movementService, IPlayerInteractionService interactionService,
            IInteractionUIService uiService)
        {
            _movementService = movementService;
            _interactionService = interactionService;
            _uiService = uiService;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Camera.main.transform.SetParent(_cameraTransform);
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            HandleMovement();
            HandleInteraction();
        }

        private void HandleMovement()
        {
            _movementService.UpdateLook(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y"),
                _cameraTransform
            );

            _movementService.UpdateMovement(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                Time.deltaTime
            );
        }

        private void HandleInteraction()
        {
            bool found = _interactionService.TryGetInteractable(false, out var interactable);

            if (found && interactable.CanInteract)
                _uiService.ShowHint(interactable);
            else
                _uiService.HideHint();

            if (!found || !Input.GetKeyDown(KeyCode.E)) return;

            if (interactable.CanInteract) interactable.OnInteract(gameObject);
        }
    }
}
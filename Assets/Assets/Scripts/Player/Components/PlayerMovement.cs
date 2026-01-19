using Coop.Player.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Player.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform _cameraTransform;

        private IPlayerMovementService _movementService;

        [Inject]
        public void Construct(IPlayerMovementService movementService) => _movementService = movementService;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnWoundedStateChanged(bool isWounded) => _movementService.SetWounded(isWounded);

        public override void OnStartLocalPlayer()
        {
            if (!Camera.main) return;

            Camera.main.transform.SetParent(_cameraTransform);
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

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
    }
}
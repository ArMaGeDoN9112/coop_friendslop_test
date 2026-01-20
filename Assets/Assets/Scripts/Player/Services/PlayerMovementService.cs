using Coop.Configs;
using UnityEngine;

namespace Coop.Player.Services
{
    public class PlayerMovementService : IPlayerMovementService
    {
        private readonly CharacterController _controller;
        private readonly PlayerConfig _config;
        private readonly Transform _transform;

        private float _verticalRotation;
        private Vector3 _velocity;
        private bool _isWounded;

        public PlayerMovementService(CharacterController controller, PlayerConfig config)
        {
            _controller = controller;
            _config = config;
            _transform = controller.transform;
        }

        public void SetWounded(bool isWounded) => _isWounded = isWounded;

        public void UpdateMovement(float inputX, float inputZ, float deltaTime)
        {
            if (_controller.isGrounded && _velocity.y < 0) _velocity.y = -2f;

            float currentSpeed = _isWounded ? _config.CrawlSpeed : _config.MoveSpeed;

            var move = _transform.right * inputX + _transform.forward * inputZ;

            _controller.Move(move * (currentSpeed * deltaTime));

            _velocity.y += _config.Gravity * deltaTime;
            _controller.Move(_velocity * deltaTime);
        }

        public void UpdateLook(float mouseX, float mouseY, Transform cameraTransform)
        {
            _transform.Rotate(Vector3.up * (mouseX * _config.MouseSensitivity));
            _verticalRotation -= mouseY * _config.MouseSensitivity;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }
    }
}
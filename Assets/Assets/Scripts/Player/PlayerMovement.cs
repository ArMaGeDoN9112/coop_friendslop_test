using Coop.Configs;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform _cameraTransform;

        private CharacterController _controller;
        private float _verticalRotation;
        private bool _isWounded;
        private Vector3 _velocity;
        private PlayerConfig _playerConfig;

        [Inject]
        public void Construct(PlayerConfig config) => _playerConfig = config;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void OnStartLocalPlayer()
        {
            if (!Camera.main) return;

            Camera.main.transform.SetParent(_cameraTransform);
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }

        public void SetWoundedState(bool wounded) => _isWounded = wounded;

        private void Update()
        {
            if (!isLocalPlayer) return;

            HandleLook();
            HandleMovement();
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * _playerConfig.MouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            float mouseY = Input.GetAxis("Mouse Y") * _playerConfig.MouseSensitivity;
            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
            _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }

        private void HandleMovement()
        {
            if (_controller.isGrounded && _velocity.y < 0) _velocity.y = -2f;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            float currentSpeed = _isWounded
                ? _playerConfig.MoveSpeed * _playerConfig.CrawlSpeedFactor
                : _playerConfig.MoveSpeed;

            var move = transform.right * x + transform.forward * z;
            _controller.Move(move * (currentSpeed * Time.deltaTime));
            _velocity.y += _playerConfig.Gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
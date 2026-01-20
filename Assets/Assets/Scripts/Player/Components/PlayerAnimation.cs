using Coop.Configs;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Player.Components
{
    [RequireComponent(typeof(NetworkAnimator))]
    public class PlayerAnimation : NetworkBehaviour
    {
        private float _dampTime;
        private Animator _animator;
        private PlayerHealth _playerHealth;
        private NetworkAnimator _networkAnimator;
        private Vector3 _lastPosition;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsWoundedHash = Animator.StringToHash("IsWounded");
        private static readonly int InteractHash = Animator.StringToHash("Interact");

        [Inject]
        public void Construct(PlayerHealth playerHealth, PlayerConfig playerConfig)
        {
            _playerHealth = playerHealth;
            _dampTime = playerConfig.AnimationDampTime;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();
        }

        public override void OnStartClient() => HandleWoundedState(_playerHealth.IsWounded);

        private void OnEnable() => _lastPosition = transform.position;

        private void Update() => AnimateMovement();

        private void AnimateMovement()
        {
            var currentPosition = transform.position;
            var positionDelta = currentPosition - _lastPosition;
            positionDelta.y = 0;

            float speed = positionDelta.magnitude / Mathf.Max(Time.deltaTime, Mathf.Epsilon);
            _animator.SetFloat(SpeedHash, speed, _dampTime, Time.deltaTime);
            _lastPosition = currentPosition;
        }

        public void HandleWoundedState(bool isWounded) => _animator.SetBool(IsWoundedHash, isWounded);

        public void TriggerInteraction() => _networkAnimator.SetTrigger(InteractHash);
    }
}
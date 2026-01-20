using UnityEngine;

namespace Coop.Player.Services
{
    public class PlayerHealthService : IPlayerHealthService
    {
        private readonly CharacterController _controller;

        public PlayerHealthService(CharacterController controller) => _controller = controller;

        public void ApplyStateEffects(bool isWounded) // TODO: refactor
        {
            if (isWounded)
            {
                _controller.height = 1.5f;
                _controller.center = new Vector3(0, 0.75f, 0);
            }
            else
            {
                _controller.height = 3.0f;
                _controller.center = new Vector3(0, 1.5f, 0);
            }
        }
    }
}
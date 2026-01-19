using UnityEngine;

namespace Coop.Player.Services
{
    public interface IPlayerMovementService
    {
        void SetWounded(bool isWounded);
        void UpdateMovement(float inputX, float inputZ, float deltaTime);
        void UpdateLook(float mouseX, float mouseY, Transform cameraTransform);
    }
}
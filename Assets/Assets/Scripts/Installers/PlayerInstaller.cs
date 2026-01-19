using Coop.Player.Components;
using Coop.Player.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private PlayerMovement _movementView;
        [SerializeField] private PlayerHealth _healthView;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerMovementService>().AsSingle();
            Container.BindInterfacesTo<PlayerHealthService>().AsSingle();
            Container.BindInterfacesTo<PlayerInteractionService>().AsSingle();

            Container.Bind<PlayerMovement>().FromInstance(_movementView).AsSingle();
            Container.Bind<PlayerHealth>().FromInstance(_healthView).AsSingle();

            Container.Bind<CharacterController>().FromInstance(_characterController).AsSingle();
            Container.Bind<Transform>().WithId("CameraRoot").FromInstance(_cameraRoot).AsCached();

            Container.Bind<NetworkIdentity>().FromComponentInHierarchy().AsSingle();
        }
    }
}
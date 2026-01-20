using Coop.Interaction;
using Coop.Interaction.UI;
using Coop.Player.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Installers
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private CharacterController _lobbyPlayerController;
        [SerializeField] private InteractionHintView _interactionHintPrefab;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterController>().FromComponentInHierarchy().AsSingle();

            Container.Bind<NetworkIdentity>().FromMethod(_ => null).AsSingle();

            Container.BindInterfacesTo<PlayerMovementService>().AsSingle();
            Container.BindInterfacesTo<PlayerInteractionService>().AsSingle();

            Container.BindInterfacesTo<InteractionUIService>().AsSingle();
            Container.Bind<InteractionHintView>().FromComponentInNewPrefab(_interactionHintPrefab).AsSingle();
        }
    }
}
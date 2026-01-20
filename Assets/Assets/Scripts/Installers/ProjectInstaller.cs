using Coop.Utils;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private NetworkManager _networkManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind(typeof(NetworkManager), typeof(CustomNetworkManager))
                .To<CustomNetworkManager>()
                .FromComponentInNewPrefab(_networkManagerPrefab)
                .AsSingle();
        }
    }
}
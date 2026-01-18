using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Configs
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private NetworkManager _networkManagerPrefab;
        [SerializeField] private GameObject _playerPrefab;

        public override void InstallBindings()
        {
            Container.Bind(typeof(NetworkManager), typeof(CustomNetworkManager))
                .To<CustomNetworkManager>()
                .FromComponentInNewPrefab(_networkManagerPrefab)
                .AsSingle();
        }
    }
}
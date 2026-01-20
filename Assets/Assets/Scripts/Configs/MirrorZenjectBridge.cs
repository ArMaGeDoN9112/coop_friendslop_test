using Coop.Utils;
using Mirror;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Coop.Configs
{
    public class MirrorZenjectBridge : IInitializable
    {
        private readonly NetworkManager _networkManager;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly IPrefabFactory _prefabFactory;

        public MirrorZenjectBridge(NetworkManager networkManager, PrefabsConfig prefabsConfig,
            IPrefabFactory prefabFactory)
        {
            _networkManager = networkManager;
            _prefabsConfig = prefabsConfig;
            _prefabFactory = prefabFactory;
        }

        public void Initialize()
        {
            NetworkClient.UnregisterPrefab(_networkManager.playerPrefab);

            NetworkClient.RegisterPrefab(_networkManager.playerPrefab,
                msg => SpawnHandler(_networkManager.playerPrefab, msg),
                UnspawnHandler);

            foreach (var prefab in _prefabsConfig.PrefabsToRegister)
                NetworkClient.RegisterPrefab(prefab,
                    msg => SpawnHandler(prefab, msg),
                    UnspawnHandler);
        }

        public void Dispose()
        {
            if (!_networkManager) return;

            foreach (var prefab in _prefabsConfig.PrefabsToRegister) NetworkClient.UnregisterPrefab(prefab);
        }

        private GameObject SpawnHandler(GameObject prefab, SpawnMessage msg) =>
            _prefabFactory.Create(prefab, null, msg.position, msg.rotation);

        private static void UnspawnHandler(GameObject spawnedObj) => Object.Destroy(spawnedObj);
    }
}
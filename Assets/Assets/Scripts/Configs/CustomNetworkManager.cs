using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Configs
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var startPos = GetStartPosition();

            var sceneContext = FindAnyObjectByType<SceneContext>();

            if (!sceneContext)
            {
                Debug.LogError("FATAL: На сцене нет SceneContext! Не могу заспавнить игрока с зависимостями.");
                return;
            }

            var sceneContainer = sceneContext.Container;
            var player =
                sceneContainer.InstantiatePrefab(playerPrefab, startPos.position, startPos.rotation, null);

            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
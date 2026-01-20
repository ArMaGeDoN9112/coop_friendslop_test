using Mirror;
using UnityEngine.SceneManagement;
using Zenject;

namespace Coop.Utils
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var startPos = GetStartPosition();

            var sceneContext = FindAnyObjectByType<SceneContext>();
            var sceneContainer = sceneContext.Container;

            var player =
                sceneContainer.InstantiatePrefab(playerPrefab, startPos.position, startPos.rotation, null);

            NetworkServer.AddPlayerForConnection(conn, player);
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            SceneManager.LoadScene("Menu"); //TODO: move to config
        }
    }
}
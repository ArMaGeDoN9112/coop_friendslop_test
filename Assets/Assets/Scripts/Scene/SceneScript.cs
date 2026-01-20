using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Scene
{
    public class SceneScript : NetworkBehaviour
    {
        private NetworkManager _networkManager;

        [Inject]
        private void Construct(NetworkManager networkManager) => _networkManager = networkManager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) ExitServer();
        }

        private void ExitServer()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
                _networkManager.StopHost();
            else if (NetworkClient.isConnected)
                _networkManager.StopClient();
            else if (NetworkServer.active) _networkManager.StopServer();
        }
    }
}
using Coop.Player.Components;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Coop.Scene
{
    public class SceneScript : NetworkBehaviour
    {
        [Header("UI References")] public TMP_Text _canvasStatusText;

        [SyncVar(hook = nameof(OnStatusTextChanged))]
        public string statusText;

        public PlayerInfo LocalPlayer { get; set; }

        private NetworkManager _networkManager;

        [Inject]
        private void Construct(NetworkManager networkManager) => _networkManager = networkManager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) ChangeScene();
            if (Input.GetKeyDown(KeyCode.Escape)) ExitServer();
        }

        private void OnStatusTextChanged(string _, string newText) => _canvasStatusText.text = newText;

        private void ChangeScene()
        {
            if (isServer)
            {
                var scene = SceneManager.GetActiveScene();

                string nextScene = scene.name == "Scene1" ? "Scene2" : "Scene1";
                _networkManager.ServerChangeScene(nextScene);
            }
            else { _canvasStatusText.text = "Only Host can change scene."; }
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
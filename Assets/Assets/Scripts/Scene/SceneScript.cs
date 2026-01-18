using Coop.Player;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Coop.Scene
{
    // Этот скрипт должен висеть на объекте в сцене (например, "GameManager" или "SceneReference")
    // У этого объекта должен быть NetworkIdentity!
    public class SceneScript : NetworkBehaviour
    {
        [Header("UI References")] public TMP_Text _canvasStatusText;
        public TMP_Text _interactionInfoText;

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

        public void SetInteractionText(bool active, string text = "")
        {
            _interactionInfoText.text = text;
            _interactionInfoText.gameObject.SetActive(active);
        }


        private void OnStatusTextChanged(string _Old, string _New) => _canvasStatusText.text = _New;

        public void ButtonSendMessage() => LocalPlayer.CmdSendPlayerMessage();

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

        [Server]
        public void ServerLog(string message) => statusText = message;
    }
}
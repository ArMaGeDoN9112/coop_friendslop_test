using Coop.Player;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) ChangeScene();
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
                NetworkManager.singleton.ServerChangeScene(nextScene);
            }
            else { _canvasStatusText.text = "Only Host can change scene."; }
        }

        [Server]
        public void ServerLog(string message) => statusText = message;
    }
}
using Coop.Scene;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Coop.Player
{
    public class PlayerInfo : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnNameChanged))]
        public string PlayerName;

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color PlayerColor = Color.white;

        [Header("Visuals")] [SerializeField] private Renderer _bodyRenderer;
        [SerializeField] private TMP_Text _nameLabel3D;

        private SceneScript _sceneScript;

        [Inject]
        public void Construct(SceneScript sceneScript) => _sceneScript = sceneScript;

        private void Update()
        {
            if (isLocalPlayer) return;

            _nameLabel3D.transform.LookAt(Camera.main.transform);
        }

        public override void OnStartLocalPlayer()
        {
            _sceneScript.LocalPlayer = this;

            string genName = "Player" + Random.Range(100, 999);
            var genColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            CmdSetupPlayer(genName, genColor);
        }

        [Command]
        private void CmdSetupPlayer(string _name, Color _col)
        {
            PlayerName = _name;
            PlayerColor = _col;

            _sceneScript.ServerLog($"{PlayerName} joined the game.");
        }

        [Command]
        public void CmdSendPlayerMessage() =>
            _sceneScript.ServerLog($"{PlayerName} says hello {Random.Range(10, 99)}");

        private void OnNameChanged(string _Old, string _New)
        {
            _nameLabel3D.text = _New;
            gameObject.name = _New;
        }

        private void OnColorChanged(Color _Old, Color _New)
        {
            _nameLabel3D.color = _New;
            _bodyRenderer.material.color = _New;
        }
    }
}
using Coop.Scene;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Coop.Player.Components
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
        private void CmdSetupPlayer(string name, Color color)
        {
            PlayerName = name;
            PlayerColor = color;
        }

        private void OnNameChanged(string _, string newName)
        {
            _nameLabel3D.text = newName;
            gameObject.name = newName;
        }

        private void OnColorChanged(Color _, Color newColor)
        {
            _nameLabel3D.color = newColor;
            _bodyRenderer.material.color = newColor;
        }
    }
}
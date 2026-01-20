using TMPro;
using UnityEngine;

namespace Coop.Interaction.UI
{
    public class InteractionHintView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _promptText;

        private readonly Vector3 _offset = new(0, 0.5f, 0);
        private Transform _anchorTransfrom;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            Hide();
        }

        public void Show(string text, Transform targetAnchor)
        {
            _promptText.text = text;
            _anchorTransfrom = targetAnchor;
            _canvas.enabled = true;
            UpdatePosition();
        }

        public void Hide()
        {
            _canvas.enabled = false;
            _anchorTransfrom = null;
        }

        private void LateUpdate()
        {
            if (!_canvas.enabled || !_anchorTransfrom) return;

            UpdatePosition();
            LookAtCamera();
        }

        private void UpdatePosition() => transform.position = _anchorTransfrom.position + _offset;

        private void LookAtCamera() =>
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                _mainCamera.transform.rotation * Vector3.up);
    }
}
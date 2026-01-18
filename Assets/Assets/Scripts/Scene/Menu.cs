using UnityEngine;
using UnityEngine.SceneManagement;

namespace Coop.Scene
{
    public class Menu : MonoBehaviour
    {
        public void LoadScene() => SceneManager.LoadScene("ConnectionScene");
    }
}
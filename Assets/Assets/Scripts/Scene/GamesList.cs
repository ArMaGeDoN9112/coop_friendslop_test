using UnityEngine;
using UnityEngine.SceneManagement;

namespace Coop.Scene
{
    public class GamesList : MonoBehaviour
    {
        public void LoadScene() => SceneManager.LoadScene("Menu");
    }
}
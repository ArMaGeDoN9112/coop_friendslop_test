using Coop.Configs;
using UnityEngine;
using Zenject;

namespace Coop.Scene
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneScript sceneScript;

        public override void InstallBindings()
        {
            Container.Bind<SceneScript>().FromInstance(sceneScript).AsSingle();
            Container.BindInterfacesTo<MirrorZenjectBridge>().AsSingle();
        }
    }
}
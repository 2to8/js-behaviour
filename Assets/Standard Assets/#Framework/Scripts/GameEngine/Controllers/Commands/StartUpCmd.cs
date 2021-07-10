using GameEngine.Controllers.Contracts;
using GameEngine.Kernel._Appliation;
using GameEngine.Kernel.Args;
using GameEngine.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEngine.Controllers.Commands
{
    public class StartUpCmd : Controller<StartUpCmd>
    {
        public override void Execute(object data)
        {
            //注册模型
            RegisterModel<TestGameTable>();

            //注册控制器
            RegisterController<EnterSceneCmd>(Defines.E_EnterScene);
            RegisterController<ExitSceneCmd>(Defines.E_ExitScene);
            RegisterController<AddLevelCmd>(Defines.E_AddLevel);
            if (!Application.isEditor && SceneManager.GetActiveScene().buildIndex != 1) {
                if (SceneManager.GetSceneByBuildIndex(1).IsValid()) {
                    GameBootstrapV1.LoadScene(1);
                }
            }
        }
    }
}
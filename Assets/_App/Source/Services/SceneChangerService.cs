using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

namespace Omega.Kulibin
{
    public sealed class SceneChangerService
    {
        private readonly SceneReference _emptyScene;

        public SceneChangerService(AppScenes appScenes)
        {
            _emptyScene = appScenes.EmptyScene;
        }

        /// <summary>
        /// Выгружает текущую сцену и переключается на sceneName
        /// </summary>
        /// <param name="sceneName">название сцены, на которую надо переключиться</param>
        public async UniTask SwitchToScene(string sceneName)
        {
            var oldScene = SceneManager.GetActiveScene();
            await AddSceneAndSwitchAsync(sceneName);
            await SceneManager.UnloadSceneAsync(oldScene);
        }

        /// <summary>
        /// Перезагружает сцену sceneName
        /// </summary>
        /// <param name="sceneName">название сцены, которую надо перезагрузить</param>
        public async UniTask ReloadScene(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                await SwitchToScene(_emptyScene.Name);
                await SwitchToScene(sceneName);
            }
            else
            {
                await SceneManager.UnloadSceneAsync(sceneName);
                await AddSceneAsync(sceneName);
            }
        }

        /// <summary>
        /// Загружает сцену sceneName и переключается на неё, не удаляя текущую
        /// </summary>
        /// <param name="sceneName">название сцены, которую надо загрузить</param>
        public async UniTask AddSceneAndSwitchAsync(string sceneName)
        {
            await AddSceneAsync(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        /// <summary>
        /// Выгружает сцену sceneName
        /// </summary>
        /// <param name="sceneName">сцена, которую надо выгрузить</param>
        public async UniTask UnloadSceneAsync(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
                await SwitchToScene(_emptyScene.Name);
            await SceneManager.UnloadSceneAsync(sceneName);
        }

        /// <summary>
        /// Загружает сцену sceneName, не переключаясь на неё
        /// </summary>
        /// <param name="sceneName">сцена, которую надо загрузить</param>
        public async UniTask AddSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
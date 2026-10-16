using IllusionPlugin;
using System;
using UnityEngine.SceneManagement;

namespace BeatSaverDownloader
{
    public class Plugin : IPlugin
    {
        public string Name => "BeatSaver Downloader";
        public string Version => "2.7.0";

        public void OnApplicationQuit()
        {
            PluginConfig.SaveConfig();
        }
        
        public void OnApplicationStart()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            PluginConfig.LoadOrCreateConfig();
        }

        private void SceneManager_sceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.name == "Menu")
            {
                BeatSaberUI.OnLoad();
                PluginUI.PluginUI.OnLoad();
            }
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
        }
    }
}

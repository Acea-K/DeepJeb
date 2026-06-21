using System;
using UnityEngine;

namespace DeepJeb.Unity.Lifecycle
{
    /// <summary>
    /// Handles cross-scene lifecycle for DeepJeb.
    /// Calls OnSaveRequested on scene changes and game saves.
    /// Survives via DontDestroyOnLoad (parent DeepJebMod handles this).
    /// </summary>
    public class ModLifecycle : MonoBehaviour
    {
        /// <summary>Callback invoked when config should be persisted.</summary>
        public Action OnSaveRequested { get; set; }

        private GameScenes _currentScene;

        private void Start()
        {
            _currentScene = HighLogic.LoadedScene;
            GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequested);
            GameEvents.onGameStateSave.Add(OnGameSaved);
            Debug.Log("[DeepJeb] Lifecycle started (scene: " + _currentScene + ").");
        }

        private void OnDestroy()
        {
            GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequested);
            GameEvents.onGameStateSave.Remove(OnGameSaved);
            OnSaveRequested?.Invoke();
            Debug.Log("[DeepJeb] Lifecycle destroyed.");
        }

        private void OnSceneLoadRequested(GameScenes targetScene)
        {
            Debug.Log("[DeepJeb] Scene: " + _currentScene + " -> " + targetScene);
            OnSaveRequested?.Invoke();
            _currentScene = targetScene;
        }

        private void OnGameSaved(ConfigNode saveData)
        {
            // Trigger only — we do NOT write into KSP's saveData
            OnSaveRequested?.Invoke();
        }
    }
}

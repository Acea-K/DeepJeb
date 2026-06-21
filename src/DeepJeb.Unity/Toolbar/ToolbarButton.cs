using System;
using KSP.UI.Screens;
using UnityEngine;

namespace DeepJeb.Unity.Toolbar
{
    /// <summary>
    /// Registers a DeepJeb button on the KSP stock toolbar (ApplicationLauncher).
    ///
    /// Created and managed by DeepJebMod — NOT auto-instantiated via [KSPAddon].
    /// Listens to onGUIApplicationLauncherReady to (re-)register the button
    /// after scene changes (since ApplicationLauncher is rebuilt per scene).
    ///
    /// Set OnToggle before registering to handle click events.
    ///
    /// Available in: SpaceCentre, Flight, TrackingStation, VAB, SPH.
    /// </summary>
    public class ToolbarButton : MonoBehaviour
    {
        /// <summary>Callback invoked when the toolbar button is clicked.</summary>
        public Action OnToggle { get; set; }

        private ApplicationLauncherButton _launcherButton;
        private Texture2D _buttonIcon;

        private static readonly ApplicationLauncher.AppScenes AllowedScenes =
            ApplicationLauncher.AppScenes.SPACECENTER |
            ApplicationLauncher.AppScenes.FLIGHT |
            ApplicationLauncher.AppScenes.TRACKSTATION |
            ApplicationLauncher.AppScenes.SPH |
            ApplicationLauncher.AppScenes.VAB;

        private void Start()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(OnLauncherReady);
            GameEvents.onGUIApplicationLauncherDestroyed.Add(OnLauncherDestroyed);

            // If launcher is already ready, register immediately
            if (ApplicationLauncher.Ready && _launcherButton == null)
                RegisterButton();
        }

        private void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(OnLauncherReady);
            GameEvents.onGUIApplicationLauncherDestroyed.Remove(OnLauncherDestroyed);
            RemoveButton();
            if (_buttonIcon != null) { Destroy(_buttonIcon); _buttonIcon = null; }
        }

        private void OnLauncherReady()
        {
            if (_launcherButton == null)
                RegisterButton();
        }

        private void OnLauncherDestroyed()
        {
            _launcherButton = null;
        }

        private void RegisterButton()
        {
            if (_launcherButton != null)
                return;

            if (_buttonIcon == null)
                _buttonIcon = CreateIcon();

            if (ApplicationLauncher.Instance == null) return;
            _launcherButton = ApplicationLauncher.Instance.AddModApplication(
                onTrue: () => OnToggle?.Invoke(),
                onFalse: () => { /* window hidden */ },
                onHover: null,
                onHoverOut: null,
                onEnable: null,
                onDisable: null,
                visibleInScenes: AllowedScenes,
                texture: _buttonIcon
            );

            Debug.Log("[DeepJeb] Toolbar button registered.");
        }

        private void RemoveButton()
        {
            if (_launcherButton != null && ApplicationLauncher.Instance != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(_launcherButton);
                _launcherButton = null;
            }
        }

        private Texture2D CreateIcon()
        {
            // Try loading custom texture from GameData
            string texPath = "DeepJeb/Textures/DeepJebIcon";
            var customTex = GameDatabase.Instance.GetTexture(texPath, false);
            if (customTex != null)
                return customTex;

            // Procedural blue circle (#4D8CD9, DeepJeb accent)
            int size = 38;
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            Color blue = new Color(0.302f, 0.549f, 0.851f, 1f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - size / 2f;
                    float dy = y - size / 2f;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist <= size / 2f - 2f)
                        tex.SetPixel(x, y, blue);
                    else
                        tex.SetPixel(x, y, Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }
    }
}

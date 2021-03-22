using System.Linq;
using System;
using BepInEx;
using HarmonyLib;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace ValheimStands
{
    [BepInPlugin("com.github.ceko.ValheimStands", "ValheimStands", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private static readonly Harmony harmony = new(typeof(Plugin).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);

        public static readonly string FolderPath = Path.GetDirectoryName(Path.Combine(Assembly.GetAssembly(typeof(Plugin)).Location));
        public static readonly string AssetPath = Path.Combine(FolderPath, "valheim-stands");
        public static readonly string PieceConfigPath = Path.Combine(FolderPath, "pieces.json");

        public static BepInEx.Logging.ManualLogSource Logger {
            get;
            private set;
        }

        private void Awake()
        {   
            Plugin.Logger = base.Logger;

            Logger.LogInfo("Applying patches.");            
            harmony.PatchAll();
            
            Logger.LogInfo($"Asset bundle loading at path {Plugin.AssetPath}");            
            foreach(GameObject piece in UnityBundle.Database.Pieces) {
                Logger.LogInfo($"Setting up {piece.name}");
                var config = AppConfig.instance.Pieces[piece.name];
                Logger.LogInfo(config);
            }
        }

        private void OnDestroy()
        {
            Logger.LogInfo("Unapplying patches.");
            harmony.UnpatchSelf();
        }
    }
}

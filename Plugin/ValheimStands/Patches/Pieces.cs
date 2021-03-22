using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using ValheimStands.Utils;
using GameConsole = Console;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace ValheimStands.Patches.Pieces {        
    
    [HarmonyPatch(typeof(ZNetScene), "Awake")]
    public static class ZNetSceneDatabaseAwake
    {
        public static void Prefix(ref ZNetScene __instance)
        {
            try {
                Plugin.Logger.LogInfo("Adding items to the ZNetScene database");
                foreach(GameObject resource in UnityBundle.Database.Pieces) {                    
                     __instance.m_prefabs.Add(resource);  
                }

                Plugin.Logger.LogInfo("Adding localization");
                // set up just english localization
                foreach(KeyValuePair<string, string> entry in AppConfig.instance.Localization["English"]) {
                    Localization.instance.AddWord(entry.Key, entry.Value);
                }   
            }catch(Exception exc) {
                Plugin.Logger.LogError(exc);
            }
        }  

        static bool piecesAdded = false;
        public static void Postfix(ref ZNetScene __instance) {
            if(piecesAdded) return;
            piecesAdded = true;

            try {
                Plugin.Logger.LogInfo("Adding pieces to the hammer.");
                var hammer = ZNetScene.instance.GetPrefab("Hammer");
                
                foreach(GameObject pieceGo in UnityBundle.Database.Pieces) {
                    Plugin.Logger.LogInfo($"Setting up {pieceGo.name}.");                    
                    var pieceConfig = AppConfig.instance.Pieces[pieceGo.name];                    
                    var piece = pieceGo.GetComponent<ValheimStands.Unity.CustomPiece>();
                    Plugin.Logger.LogInfo($"Calling LoadConfig on {piece}.");
                    piece.LoadConfig(pieceConfig);
                    Plugin.Logger.LogInfo($"Adding {piece} to the hammer's piece table.");
                    hammer.GetComponent<ItemDrop>().m_itemData.m_shared.m_buildPieces.m_pieces.Add(pieceGo);
                }                
            }catch(Exception exc) {
                Plugin.Logger.LogError(exc);
            } 
        }      
    }

    
}

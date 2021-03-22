using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace ValheimStands.Patches.Persistence {
    
    [HarmonyPatch(typeof(ZDOMan), "Load")]
    public static class LoadCustomPieces {

        public static void Postfix(ZDOMan __instance) {
            // proof of concept persistence, don't want to include or test it now
            return;

            var fileName = ZNet.m_world.m_name + ".armorstands.db"; 
            var fullFilePath = World.GetWorldSavePath() + "/" + fileName;
            Plugin.Logger.LogInfo($"Trying to load save file at {fullFilePath}");
            if(!File.Exists(fullFilePath)) return;

            try {
                using(BinaryReader reader = new BinaryReader(File.Open(fullFilePath, FileMode.Open))) {                    
                    var worldVersion = reader.ReadInt32();
                    Plugin.Logger.LogInfo($"World version: ${worldVersion}");
                    var zdoCount = reader.ReadInt32();
                    Plugin.Logger.LogInfo($"{zdoCount} zdos in file");

                    for(var i=0;i<zdoCount;i++) {
                        var zdo = new ZDO();
                        zdo.m_uid = new ZDOID(reader);
                        
                        var packageSize = reader.ReadInt32();
                        var zpkg = new ZPackage(reader.ReadBytes(packageSize));                        
                        zdo.Load(zpkg, worldVersion);

                        Plugin.Logger.LogInfo($"Loaded zdo {zdo.m_uid}.");
                        if(__instance.GetZDO(zdo.m_uid) == null) {
                            Plugin.Logger.LogInfo("zdo not found, adding to database.");
                            __instance.m_objectsByID.Add(zdo.m_uid, zdo);
                            __instance.AddToSector(zdo, zdo.GetSector());
                        }else{
                            Plugin.Logger.LogInfo("zdo found in database, skipping.");
                        }
                    }                                        
                }
            }catch(Exception exc) {
                Plugin.Logger.LogInfo(exc);
            }
        }
    }

    [HarmonyPatch(typeof(ZNet), "SaveWorld")]
    public static class SaveCustomPieces {

        public static void Postfix() {
            try {
                // proof of concept persistence, don't want to include or test it now
                return;

                //TODO: Only do this if we're the server.
                Plugin.Logger.LogInfo("Persisting armorstand db");

                var armorStandsZDOs = new List<ZDO>();
                ZDOMan.instance.GetAllZDOsWithPrefab("FullArmorStand", armorStandsZDOs);

                var zpkg = new ZPackage();
                var fileName = ZNet.m_world.m_name + ".armorstands.db"; //make a variable

                using(BinaryWriter writer = new BinaryWriter(File.Open(World.GetWorldSavePath() + "/" + fileName, FileMode.OpenOrCreate))) {
                    writer.Write(Version.m_worldVersion); //int32
                    writer.Write(armorStandsZDOs.Count); //int32                        

                    foreach(var zdo in armorStandsZDOs) {
                        writer.Write(zdo.m_uid.userID); //long
                        writer.Write(zdo.m_uid.id); //uint32

                        zpkg.Clear();
                        zdo.Save(zpkg);

                        byte[] array = zpkg.GetArray();                    
                        writer.Write(array.Length);//int32
                        writer.Write(array);                
                        Plugin.Logger.LogInfo("Persisting armor stand with id " + zdo.m_uid);
                    }
                }
            }catch(Exception exc) {
                Plugin.Logger.LogError("Error saving backup data: " + exc);
            }
        }

    }

}
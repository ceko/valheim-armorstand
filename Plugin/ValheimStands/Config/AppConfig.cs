using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

namespace ValheimStands {

    [Serializable]
    public class AppConfig {

        private static AppConfig _instance;
        public static AppConfig instance {
            get {
                if(_instance == null) {                                        
                    _instance = JsonMapper.ToObject<ValheimStands.AppConfig>(System.IO.File.ReadAllText(Plugin.PieceConfigPath));                    
                }

                return _instance;
            }
        }

        
        public Dictionary<string, ValheimStands.Unity.LazyPieceSettings> Pieces;
        public Dictionary<string, Dictionary<string, string>> Localization;
    }

}
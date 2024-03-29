using System;
using UnityEngine;
using System.Collections.Generic;


namespace ValheimStands {

    public static class UnityBundle {
        private static readonly Lazy<AssetBundle> bundle = new Lazy<AssetBundle>(() => AssetBundle.LoadFromFile(Plugin.AssetPath));
        public static AssetBundle Instance {
            get {
                return bundle.Value;
            }
        }
        
        public static readonly BundleResource<GameObject> Mod = new BundleResource<GameObject>("Mod");

        public static ValheimStands.Unity.Database Database { 
            get {                                
                return Mod.Value.GetComponent<ValheimStands.Unity.Database>();
            }
        }        
    }
       
    public class BundleResource<T> where T : UnityEngine.Object {

        private string _assetPath;

        private T _value;
        public T Value {
            get {
                if(_value == null) {
                    _value = UnityBundle.Instance.LoadAsset<T>(_assetPath);
                    _value.name = _assetPath;                    
                }

                return _value;
            }
        } 

        public BundleResource(string assetPath) {
            _assetPath = assetPath;
        }

    }   
    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ValheimStands.Unity {
    [HidePropertiesInInspector(
        "m_resources",
        "m_placeEffect",
        "m_craftingStation"
    )]
    public class CustomPiece : Piece { 

        public void LoadConfig(LazyPieceSettings settings) {
            m_resources = new Piece.Requirement[settings.requirements.Length];
            UnityEngine.Debug.Log($"Enumerating reqs {settings.requirements}");
            foreach(var tuple in Utility.enumerate(settings.requirements)) {
                var itemDef = tuple.Item1; 
                var idx = tuple.Item2;
                UnityEngine.Debug.Log($"Resolving {itemDef.item}");

                m_resources[idx] = new Piece.Requirement {
                  m_resItem = ZNetScene.instance.GetPrefab(itemDef.item).GetComponent<ItemDrop>(),
                  m_amount = (int)itemDef.amount,
                  m_recover = true
                };
            }

            UnityEngine.Debug.Log($"Enumerating place effects {settings.placeEffects}");
            m_placeEffect.m_effectPrefabs = new EffectList.EffectData[settings.placeEffects.Length];
            foreach(var tuple in Utility.enumerate(settings.placeEffects)) {
                var fxDef = tuple.Item1; 
                var idx = tuple.Item2;
                UnityEngine.Debug.Log($"Resolving {fxDef.effect}");

                m_placeEffect.m_effectPrefabs[idx] = new EffectList.EffectData {
                    m_prefab = ZNetScene.instance.GetPrefab(fxDef.effect),
                    m_enabled = true
                };
            }
            
            UnityEngine.Debug.Log($"Resolving {settings.craftingStation.station}");
            if(settings.craftingStation != null) {
                m_craftingStation = ZNetScene.instance.GetPrefab(settings.craftingStation.station).GetComponent<CraftingStation>();
            }

            var wearNTear = GetComponent<WearNTear>();
            UnityEngine.Debug.Log($"Enumerating destroyed effects {settings.destroyedEffects}");
            wearNTear.m_destroyedEffect.m_effectPrefabs = new EffectList.EffectData[settings.destroyedEffects.Length];
            foreach(var tuple in Utility.enumerate(settings.destroyedEffects)) {
                var fxDef = tuple.Item1; 
                var idx = tuple.Item2;
                UnityEngine.Debug.Log($"Resolving {fxDef.effect}");

                wearNTear.m_destroyedEffect.m_effectPrefabs[idx] = new EffectList.EffectData {
                    m_prefab = ZNetScene.instance.GetPrefab(fxDef.effect),
                    m_enabled = true
                };
            }
            
            UnityEngine.Debug.Log($"Enumerating hit effects {settings.hitEffects}");
            wearNTear.m_hitEffect.m_effectPrefabs = new EffectList.EffectData[settings.hitEffects.Length];
            foreach(var tuple in Utility.enumerate(settings.hitEffects)) {
                var fxDef = tuple.Item1; 
                var idx = tuple.Item2;
                UnityEngine.Debug.Log($"Resolving {fxDef.effect}");

                wearNTear.m_hitEffect.m_effectPrefabs[idx] = new EffectList.EffectData {
                    m_prefab = ZNetScene.instance.GetPrefab(fxDef.effect),
                    m_enabled = true
                };
            }

            if(settings.itemStand != null) {
                UnityEngine.Debug.Log("Resolving itemStand settings");
                var itemStand = GetComponent<ArmorStand>();
                itemStand.m_effects.m_effectPrefabs = new EffectList.EffectData[settings.itemStand.effects.Length];
                foreach(var tuple in Utility.enumerate(settings.itemStand.effects)) {
                    var fxDef = tuple.Item1; 
                    var idx = tuple.Item2;
                    UnityEngine.Debug.Log($"Resolving {fxDef.effect}");

                    itemStand.m_effects.m_effectPrefabs[idx] = new EffectList.EffectData {
                        m_prefab = ZNetScene.instance.GetPrefab(fxDef.effect),
                        m_enabled = true
                    };
                }
            }
        }

    } 
}
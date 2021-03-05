using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ValheimStands.Unity {
        
    [Serializable]
    public class LazyPieceSettings  {
        public string name;
        public LazyRequirement[] requirements = new LazyRequirement[0];
        public LazyEffects[] placeEffects = new LazyEffects[0];
        public LazyEffects[] destroyedEffects = new LazyEffects[0];
        public LazyEffects[] hitEffects = new LazyEffects[0];
        public ItemStandSettings itemStand;

        public LazyCraftingStation craftingStation;
    } 

    [Serializable]
    public class ItemStandSettings {
        public LazyEffects[] effects = new LazyEffects[0];
    }

    [Serializable]
    public class LazyEffects {
        public string effect;        
    }

    [Serializable]
    public class LazyCraftingStation {
        public string station;        
    }

    [Serializable]
    public class LazyRequirement {
        public string item;
        public uint amount;
    }
}
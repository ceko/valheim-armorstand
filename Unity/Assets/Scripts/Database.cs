using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ValheimStands.Unity {

    public class Database : MonoBehaviour 
    {
        [SerializeField]
        public List<GameObject> Pieces;

        [SerializeField]
        public List<RecipeExtension> Recipes;
    } 

}

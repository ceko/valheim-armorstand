﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ValheimStands.Unity {
    public class HidePropertiesInInspectorAttribute : System.Attribute
    {
    
        private string[] _props;
    
        public HidePropertiesInInspectorAttribute(params string[] props)
        {
            _props = props;
        }
    
        public string[] HiddenProperties
        {
            get { return _props; }
        }
    
    }
}
 
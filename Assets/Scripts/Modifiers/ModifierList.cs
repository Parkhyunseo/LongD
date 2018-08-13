using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modifiers
{
    [SerializeField]
    public class ModifierList
    {
        [SerializeField]
        bool _autoIncludeItsModifiers;
        [SerializeField]
        List<Modifier> _modifiers = new List<Modifier>();

        public bool AutoIncludeItsModifiers
        {
            get
            {
                return _autoIncludeItsModifiers;
            }
        }

        public List<Modifier> modifiers
        {
            get
            {
                return _modifiers;
            }
        }
         
    }
}

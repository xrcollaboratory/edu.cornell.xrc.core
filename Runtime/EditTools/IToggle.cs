using System;
using UnityEngine;

namespace XRC.Core
{

    public interface IToggle 
    {
        public event Action<bool> onToggle;
        
    }

}

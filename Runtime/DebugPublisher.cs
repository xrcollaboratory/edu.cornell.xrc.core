using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Core.Utilities
{
    public class DebugPublisher : MonoBehaviour
    {

        public static event Action m_ToggleDebug;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ToggleDebug()
        {
            m_ToggleDebug?.Invoke();
        }
    }
}
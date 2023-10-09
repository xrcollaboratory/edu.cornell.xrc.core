using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XRC.Core.Utilities
{
    [RequireComponent(typeof(DebugPublisher))]
    public class DebugPublisherInput : MonoBehaviour
    {

        private DebugPublisher m_DebugPublisher;
        
        [SerializeField]
        private InputActionProperty m_ToggleDebugAction;

    
        // Start is called before the first frame update
        void Start()
        {
            m_DebugPublisher = GetComponent<DebugPublisher>();

            m_ToggleDebugAction.action.performed += _ => m_DebugPublisher.ToggleDebug();
        }

        private void OnEnable()
        {
            m_ToggleDebugAction.action.Enable();
        }

        private void OnDisable()
        {
            m_ToggleDebugAction.action.Disable();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
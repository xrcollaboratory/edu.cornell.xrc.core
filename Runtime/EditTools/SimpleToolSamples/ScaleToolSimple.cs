using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Core
{
    public class ScaleToolSimple : MonoBehaviour, IEditTool
    {
        public event Action runStarted;
        public event Action runStopped;
        
        [SerializeField]
        private GameObject m_EditObject;
        
        /// <summary>
        /// The object being edited.
        /// </summary>
        public GameObject editObject
        {
            get => m_EditObject;
            set => m_EditObject = value;
        }
        
        private bool m_IsRunning;
        /// <summary>
        /// Indicates whether the tool is currently running or not. 
        /// </summary>
        public bool isRunning => m_IsRunning;
        
        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        
        /// <summary>
        /// The local scale of the object being edited.
        /// </summary>
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }
        

        private void Update()
        {
            
            if (!(m_EditObject == null) && m_IsRunning)
            {
                m_EditObject.transform.localScale = m_Scale;
            }
        }

        public void StartRun()
        {
            m_IsRunning = true;
            m_Scale = editObject.transform.localScale;
            runStarted?.Invoke();
        }

        public void StopRun()
        {
            m_IsRunning = false;
            m_EditObject = null;
            runStopped?.Invoke();
        }

        public void ToggleRun()
        {
            if (!m_IsRunning)
            {
                StartRun();
            }
            else
            {
                StopRun();
            }
        }
    }
}

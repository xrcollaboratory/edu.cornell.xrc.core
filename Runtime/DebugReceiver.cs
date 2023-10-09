using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Core.Utilities
{
    public class DebugReceiver : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_ToggleObjects = new List<GameObject>();

        
        // Start is called before the first frame update
        void Start()
        {
            DebugPublisher.m_ToggleDebug += ToggleDebug;
        }

        private void ToggleDebug()
        {
            foreach (var toggleObject in m_ToggleObjects)
            {
                toggleObject.SetActive(!toggleObject.activeSelf);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
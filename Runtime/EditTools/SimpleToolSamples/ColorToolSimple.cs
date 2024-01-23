using System;
using UnityEngine;

namespace XRC.Core
{
    public class ColorToolSimple : MonoBehaviour, IEditTool, IToggle
    {
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

        private bool m_IsOn;

        public event Action onToggle;

        /// <summary>
        /// Indicates whether the tool is currently running or not. 
        /// </summary>
        public bool isOn => m_IsOn;

        [SerializeField]
        private Color m_Color;
        /// <summary>
        /// The color of the object being edited. Set this value to change its color.
        /// </summary>
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        private Material m_Material;

        private void Update()
        {
            // if (m_IsRunning && m_EditObject != null)
            if (!(m_EditObject == null) && m_IsOn)
            {
                m_Material.color = m_Color;
            }
        }

        public bool isRunning { get; }

        /// <summary>
        /// Starts running the tool and prepares for color editing. Gets the material color of the edit object and sets the component's <see cref="color"/> to its value.
        /// </summary>
        public void StartRun()
        {
            // If there currently is no assigned object to be edited, then do  nothing
            if (m_EditObject == null)
            {
                return;
            }
            m_Material = m_EditObject.GetComponent<MeshRenderer>().material;
            m_Color = m_Material.color;

            m_IsOn = true;
        }

        /// <summary>
        /// Ends the run and resets the edit object to null and the tool color to black.
        /// </summary>
        public void StopRun()
        {
            m_EditObject = null;
            m_Color = Color.black;

            m_IsOn = false;
        }

        /// <summary>
        /// Toggles the run condition of the color tool
        /// </summary>
        public void ToggleRun()
        {
            if (!m_IsOn)
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

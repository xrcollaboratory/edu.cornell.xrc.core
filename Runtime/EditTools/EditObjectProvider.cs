using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Core
{
    /// <summary>
    /// EditObjectProvider is a MonoBehaviour that provides functionality to select and interact with a target object.
    /// </summary>
    /// <remarks>
    /// This component is used in edit tools that implement the <see cref="IEditTool"/> interface, such as XRC Mesh Tool, XRC Scale Tool, and XRC Color Tool, for providing the game object to be edited.
    /// </remarks>
    public class EditObjectProvider : MonoBehaviour 
    {
        [SerializeField]
        private XRBaseInteractor m_Interactor;

        [SerializeField]
        private bool m_SnapBack = true;

        [SerializeField]
        private bool m_StartEditOnSet = true;
        
        private GameObject m_EditObject;
        private IEditTool m_EditTool;
        private Vector3 m_InitialPosition;
        private Quaternion m_InitialRotation;

        private IXRSelectInteractable m_Interactable;
        
        /// <summary>
        /// The interactor responsible for selecting the object to be interested.
        /// </summary>
        public XRBaseInteractor interactor
        {
            get => m_Interactor;
            set => m_Interactor = value;
        }

        /// <summary>
        /// The game object that is being edited. This is provided by the interactor.
        /// </summary>
        
        public GameObject editObject => m_EditObject;
        

        private void OnEnable()
        {
            
            m_EditTool = GetComponent<IEditTool>();


            if (m_EditTool != null)
            {
                m_EditTool.onToggle += ToggleEditObject;
            }
            else
            {
                Debug.LogWarning("EditObjectProvider : OnEnable : m_EditTool is null");
            }
            m_Interactor.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            if (m_EditTool != null)
            {
                m_EditTool.onToggle -= ToggleEditObject;
            }

            m_Interactor.selectEntered.RemoveListener(OnSelectEntered);
        }
        
        public void ProvideEditObject()
        {
            if (m_Interactor.hasSelection)
            {

                // Get the most recently selected interactable
                var interactables = m_Interactor.interactablesSelected;
                m_Interactable = interactables[interactables.Count - 1];

                // Get the game object containing the selected interactable
                m_EditObject = m_Interactable.transform.gameObject;

                if (m_SnapBack)
                {
                    // Move object back to its grabbed position and orientation
                    editObject.transform.position = m_InitialPosition;
                    editObject.transform.rotation = m_InitialRotation;
                }

                // Deselect the object and disable the interactable so it can't be selected while the object is a target object
                m_Interactor.interactionManager.CancelInteractableSelection(m_Interactable);
                ((XRGrabInteractable)m_Interactable).enabled = false;


                m_EditTool.editObject = editObject;
                if (m_StartEditOnSet)
                {
                    m_EditTool.StartRun();
                }
            }
        }

        public void RemoveEditObject()
        {
            ((XRGrabInteractable)m_Interactable).enabled = true;

            m_EditObject = null;
            if (m_StartEditOnSet)
            {
                m_EditTool.StopRun();
                m_EditTool.editObject = null;
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            var interactable = args.interactableObject;
            m_InitialPosition = interactable.transform.position;
            m_InitialRotation = interactable.transform.rotation;
        }
        
        public void ToggleEditObject(bool isEditing)
        {
            
            if(isEditing)
            {
                ProvideEditObject();
            }
            else
            {
                RemoveEditObject();
            }
        }

      
    }
}

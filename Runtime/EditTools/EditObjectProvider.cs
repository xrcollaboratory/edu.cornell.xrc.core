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
    /// This component is used in edit tools that implement the <see cref="IEditTool"/> interface, such as
    /// XRC Mesh Tool, XRC Scale Tool, and XRC Color Tool, for providing the game object to be edited.
    /// </remarks>
    public class EditObjectProvider : MonoBehaviour 
    {
        [SerializeField]
        private XRBaseInteractor m_Interactor;

        [SerializeField]
        private bool m_SnapBack = true;

        [SerializeField]
        private bool m_StartEditOnSet = true;
        
        
        [SerializeField]
        private InputActionProperty m_SetEditObject;
        
        
        [SerializeField]
        private GameObject m_EditObject;
        
        // Private Fields
        private IEditTool m_EditTool;
        private Vector3 m_InitialPosition;
        private Quaternion m_InitialRotation;
        private IXRSelectInteractable m_Interactable;
        private bool m_IsRunning;
        
        /// <summary>
        /// Event action when an object is scaled along X axis.
        /// </summary>
        public static event Action<GameObject> objectSelected; 
        
        /// <summary>
        /// The interactor responsible for selecting the object to be interested.
        /// </summary>
        public XRBaseInteractor interactor
        {
            get => m_Interactor;
            set => m_Interactor = value;
        }

        /// <summary>
        /// The object being edited.
        /// </summary>
        public GameObject editObject
        {
            get => m_EditObject;
            set => m_EditObject = value;
        }

        public bool isRunning
        {
            get => m_IsRunning;
            set => m_IsRunning = value;
        }

        private void Start()
        {
            m_EditTool = GetComponent<IEditTool>();
            m_SetEditObject.action.performed += _ => ToggleRun();
        }
        
        private void OnEnable()
        {
            m_SetEditObject.action.Enable();
            m_EditTool = GetComponent<IEditTool>();
            m_Interactor.selectEntered.AddListener(OnSelectEntered);
            m_Interactor.selectExited.AddListener(OnSelectExited);
        }
        
        private void OnDisable()
        {
            m_SetEditObject.action.Disable();
            m_Interactor.selectEntered.RemoveListener(OnSelectEntered);
            m_Interactor.selectExited.RemoveListener(OnSelectExited);
        }
        
        
        /// <summary>
        /// Provide the edit object to the edit tool.
        /// </summary>
        public void StartRun()
        {
            if (m_Interactor.hasSelection)
            {
                // Get the most recently selected interactable
                var interactables = m_Interactor.interactablesSelected;
                m_Interactable = interactables[interactables.Count - 1];
                
                // Get the game object containing the selected interactable
                m_EditObject = m_Interactable.transform.gameObject;

                // Deselect the object and disable the interactable so it can't be selected while the object is a target object
                m_Interactor.interactionManager.CancelInteractableSelection(m_Interactable);
                
                ((XRGrabInteractable)m_Interactable).enabled = false;
                
                if (m_SnapBack)
                {
                    // Move object back to its grabbed position and orientation
                    m_EditObject.transform.position = m_InitialPosition;
                    m_EditObject.transform.rotation = m_InitialRotation;
                }
                else
                {
                    m_EditTool.editObject = m_EditObject;
                    if (m_StartEditOnSet)
                    {
                        m_EditTool.StartRun();
                    }   
                }
                
            }
            else
            {
                if(m_EditTool == null)
                {
                    Debug.LogWarning(this.name +"EditObjectProvider : ProvideEditObject : m_EditTool is null");
                }
                else
                {
                    if(m_EditObject == null)
                    {
                        Debug.LogWarning(this.name +"EditObjectProvider : ProvideEditObject : m_EditObject is null");
                        return; // FIXME
                    }
                    
                    m_EditTool.editObject = m_EditObject;
                    if (m_StartEditOnSet)
                    {
                        m_EditObject.GetComponent<XRGrabInteractable>().enabled = false;
                        m_EditTool.StartRun();
                    }
                }
            }
        }

        /// <summary>
        /// Remove the edit object from the edit tool.
        /// </summary>
        public void StopRun()
        {
            
            // Check for null
            if (m_EditObject == null)
            {
                // It happens when input moderation calls StopRun on a tool before the object has been selected. 
                return;
            }
            m_EditObject.GetComponent<XRGrabInteractable>().enabled = true;
            m_EditTool.editObject = m_EditObject;
            m_EditTool.StopRun();
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            var interactable = args.interactableObject;
            if (interactable.transform.gameObject.name.Contains("Handle")) return;
            m_InitialPosition = interactable.transform.position;
            m_InitialRotation = interactable.transform.rotation;
            objectSelected?.Invoke(interactable.transform.gameObject);
        }
        
        private void OnSelectExited(SelectExitEventArgs arg0)
        {
            // m_EditObject = null;
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

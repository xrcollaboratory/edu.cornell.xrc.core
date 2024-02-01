using System;
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
    /// 
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
        /// The interactor responsible for selecting the object to be interested.
        /// </summary>
        public XRBaseInteractor interactor
        {
            get => m_Interactor;
            set => m_Interactor = value;
        }

        /// <summary>
        /// The object being edited. Anytime this property is set, the editObject of the EditTool is also set.
        /// </summary>
        public GameObject editObject
        {
            get => m_EditObject;
            set
            {
                m_EditObject = value;
                // Set the editObject of ScaleTool when this property is set
                if (m_EditTool != null)
                {
                    m_EditTool.editObject = m_EditObject;
                }
            }
        }

        
        /// <summary>
        /// Indicates whether the tool is currently running or not.
        /// </summary>
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

        private void Update()
        {
            CheckObjectDestroyed();
        }

        // Handles cases when the SerializedObject of SerializedProperty has been Disposed while the tool is running. 
        private void CheckObjectDestroyed()
        {
            if (!m_EditObject)
            {
                editObject = null; 
                m_IsRunning = false;
            }
        }

        private void OnEnable()
        {
            m_SetEditObject.action.Enable();
            m_EditTool = GetComponent<IEditTool>();
            m_Interactor.selectEntered.AddListener(OnSelectEntered);
        }
        
        private void OnDisable()
        {
            m_SetEditObject.action.Disable();
            m_Interactor.selectEntered.RemoveListener(OnSelectEntered);
        }
        
        
        /// <summary>
        /// Start the edit tool and provide the edit object based on the recently selected interactable.
        /// If the SnapBack property is set to true, the object will be moved back to its grabbed position and orientation.
        /// If the StartEditOnSet property is set to true, the edit tool will be started immediately after the object has been set.
        /// </summary>
        public void StartRun()
        {
            if (m_Interactor.hasSelection)
            {
                // Get the most recently selected interactable
                var interactables = m_Interactor.interactablesSelected;
                m_Interactable = interactables[interactables.Count - 1];
                
                // Get the game object containing the selected interactable
                editObject = m_Interactable.transform.gameObject;
                
                
                // Deselect the object and disable the interactable so it can't be selected while the object is a target object
                m_Interactor.interactionManager.CancelInteractableSelection(m_Interactable);
                
                ((XRGrabInteractable)m_Interactable).enabled = false;
                
                if (m_SnapBack)
                {
                    // Move object back to its grabbed position and orientation
                    m_EditObject.transform.position = m_InitialPosition;
                    m_EditObject.transform.rotation = m_InitialRotation;
                }
                
                if(m_EditTool == null)
                {
                    Debug.LogWarning(this.name +"EditObjectProvider : ProvideEditObject : m_EditTool is null");
                    return;
                }
                if (m_StartEditOnSet)
                {
                    m_EditTool.StartRun();
                }   
            }
            else
            {
                if(m_EditTool == null)
                {
                    Debug.LogWarning(this.name +"EditObjectProvider : ProvideEditObject : m_EditTool is null");
                    return;
                }
                
                if(m_EditObject == null)
                {
                    Debug.LogWarning(this.name +"EditObjectProvider : ProvideEditObject : m_EditObject is null");
                    return; 
                }
                    
                if (m_StartEditOnSet)
                {
                    m_EditObject.GetComponent<XRGrabInteractable>().enabled = false;
                    m_EditTool.StartRun();
                }
                
            }
        }

        /// <summary>
        /// Remove the edit object from the edit tool and enables the interactable so it can be selected again.
        /// Stops the run on the edit tool. 
        /// </summary>
        public void StopRun()
        {
            // Check for null, it can happen when input moderation calls StopRun on a tool before the object has been selected. 
            if (m_EditObject == null)
            {
                return;
            }
            m_EditObject.GetComponent<XRGrabInteractable>().enabled = true;
            m_EditTool.StopRun();
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            var interactable = args.interactableObject;
            if (interactable.transform.gameObject.name.Contains("Handle")) return;
            m_InitialPosition = interactable.transform.position;
            m_InitialRotation = interactable.transform.rotation;
        }
        
        
        /// <summary>
        /// Toggles the run condition of the edit tool.
        /// </summary>
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

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
        private GameObject m_EditObject;
        
        // Private Fields
        private IEditTool m_EditTool;
        private Vector3 m_InitialPosition;
        private Quaternion m_InitialRotation;
        private IXRSelectInteractable m_Interactable;
        private bool m_IsEditing;
        
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

        public bool isEditing
        {
            get => m_IsEditing;
            set => m_IsEditing = value;
        }

        private void Start()
        {
            m_EditTool = GetComponent<IEditTool>();
        }
        
        private void OnEnable()
        {
            m_EditTool = GetComponent<IEditTool>();
            if (m_EditTool != null)
            {
                m_EditTool.toggled += ToggleRun;
            }
            else
            {
                Debug.LogWarning(this.name +"EditObjectProvider : OnEnable : m_EditTool is null");
            }
            m_Interactor.selectEntered.AddListener(OnSelectEntered);
            m_Interactor.selectExited.AddListener(OnSelectExited);
        }

   

        private void OnDisable()
        {
            if (m_EditTool != null)
            {
                m_EditTool.toggled -= ToggleRun;
            }
            
            m_Interactor.selectEntered.RemoveListener(OnSelectEntered);
            m_Interactor.selectExited.RemoveListener(OnSelectExited);

        }
        
        
        /// <summary>
        /// Provide the edit object to the edit tool.
        /// </summary>
        public void ProvideEditObject()
        {
            //Debug.Log("Providing Edit Object." + m_Interactor.hasSelection);
            if (m_Interactor.hasSelection)
            {
                // Get the most recently selected interactable
                var interactables = m_Interactor.interactablesSelected;
                m_Interactable = interactables[interactables.Count - 1];
     

                // Get the game object containing the selected interactable
                m_EditObject = m_Interactable.transform.gameObject;

                // Deselect the object and disable the interactable so it can't be selected while the object is a target object
                m_Interactor.interactionManager.CancelInteractableSelection(m_Interactable);
                
                
                // Debug.Log("Disabling Interactable.");
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
                }
                else
                {
                    Debug.Log(this.name + "EditObjectProvider : ProvideEditObject : StartRun");

                    if(m_EditObject == null)
                    {
                        Debug.LogWarning(this.name +"EditObjectProvider : ProvideEditObject : m_EditObject is null");
                    }
      
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
                    }
                    
                    
                    m_EditTool.editObject = m_EditObject;
                    if (m_StartEditOnSet)
                    {
                        m_EditObject.GetComponent<XRGrabInteractable>().enabled = false;
                        Debug.Log(this.name +"EditObjectProvider : ProvideEditObject : StartRun");

                        m_EditTool.StartRun();
                    }
                    
                }

            }
        }

        /// <summary>
        /// Remove the edit object from the edit tool.
        /// </summary>
        public void RemoveEditObject()
        {
            
            
            // Check for null
            if (m_EditObject == null)
            {
                Debug.LogError("StartRun : This should not happen!!!!");
                return;
            }
            
            
            Debug.Log(this.name +"Enabling Interactable.");
            m_EditObject.GetComponent<XRGrabInteractable>().enabled = true;
            if (m_StartEditOnSet)
            {
                Debug.Log(this.name +"EditObjectProvider : RemoveEditObject : StopRun");
                if (m_EditTool != null)
                {
                    m_EditTool.StopRun();

                }
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            
            Debug.Log(this.name + " OnSelectEntered"+ args.interactableObject.transform.gameObject.name  );
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

        /// <summary>
        /// Used to toggle the run condition of the edit tool.
        /// </summary>
        /// <param name="isEdit"></param>
        public void ToggleRun(bool isEdit)
        {
            isEditing = isEdit;
            if(isEdit)
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

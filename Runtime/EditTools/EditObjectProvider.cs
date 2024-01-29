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
        
        
        [SerializeField]
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

        public bool isEditing
        {
            get => m_IsEditing;
            set => m_IsEditing = value;
        }

        private bool m_IsEditing;

 

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
                Debug.LogWarning("EditObjectProvider : OnEnable : m_EditTool is null");
            }

            m_Interactor.selectEntered.AddListener(OnSelectEntered);
            m_Interactor.selectExited.AddListener(OnSelectExited);

        }


        private void OnDisable()
        {
            // m_SetEditObject.action.Disable();

            
            Debug.Log("EditObjectProvider : OnDisable : m_EditTool : " + m_EditTool);
            if (m_EditTool != null)
            {
                Debug.Log("EditObjectProvider : OnDisable : m_EditTool is not null");
                m_EditTool.toggled -= ToggleRun;
            }
            
            m_Interactor.selectEntered.RemoveListener(OnSelectEntered);
            m_Interactor.selectExited.RemoveListener(OnSelectExited);

        }
        
        public void RequestEditObject(IEditTool tool)
        {
            
            Debug.Log("EditObjectProvider : ProvideEditObject : m_Interactor : " + m_Interactor);
            if (m_Interactor.hasSelection)
            {

                Debug.Log("EditObjectProvider : ProvideEditObject : m_Interactor has selection");
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

                
                if (tool == null)
                {
                    Debug.LogWarning("EditObjectProvider : ProvideEditObject : m_EditTool is null");
                }
                else
                {
                    tool.editObject = m_EditObject;
                    if (m_StartEditOnSet)
                    {
                        Debug.Log("EditObjectProvider : ProvideEditObject : StartRun");
                        tool.StartRun();
                    }

                }
            }
        }
        
        public void ProvideEditObject()
        {
            
            Debug.Log("EditObjectProvider : ProvideEditObject : m_Interactor : " + m_Interactor);
            if (m_Interactor.hasSelection)
            {

                Debug.Log("EditObjectProvider : ProvideEditObject : m_Interactor has selection");
                // Get the most recently selected interactable
                var interactables = m_Interactor.interactablesSelected;
                m_Interactable = interactables[interactables.Count - 1];
                ((XRGrabInteractable)m_Interactable).enabled = false;

                // Get the game object containing the selected interactable
                m_EditObject = m_Interactable.transform.gameObject;

                // Deselect the object and disable the interactable so it can't be selected while the object is a target object
                m_Interactor.interactionManager.CancelInteractableSelection(m_Interactable);
                
                
                if (m_SnapBack)
                {
                    // Move object back to its grabbed position and orientation
                    m_EditObject.transform.position = m_InitialPosition;
                    m_EditObject.transform.rotation = m_InitialRotation;
                }

                if(m_EditTool == null)
                {
                    Debug.LogWarning("EditObjectProvider : ProvideEditObject : m_EditTool is null");
                }
                else
                {
                    m_EditTool.editObject = m_EditObject;
                    if (m_StartEditOnSet)
                    {
                        Debug.Log("EditObjectProvider : ProvideEditObject : StartRun");
                        m_EditTool.StartRun();
                    }
                    
                }
                
            }
        }

        public void RemoveEditObject()
        {
            
            // Debug.Log("EditObjectProvider : RemoveEditObject : m_EditObject : " + m_EditObject);
            
            if(m_EditObject == null)
            {
                Debug.Log("EditObjectProvider : RemoveEditObject : m_EditObject is null");
                return;
            }

            m_EditObject.GetComponent<XRGrabInteractable>().enabled = true;
            if (m_StartEditOnSet)
            {
                Debug.Log("EditObjectProvider : RemoveEditObject : StopRun");
                if (m_EditTool != null)
                {
                    m_EditTool.StopRun();

                }
            }

            ((XRGrabInteractable)m_Interactable).enabled = true;

        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            

            var interactable = args.interactableObject;
            
            if (interactable.transform.gameObject.name.Contains("Handle")) return;

            
            
            m_InitialPosition = interactable.transform.position;
            m_InitialRotation = interactable.transform.rotation;
            

        }
        
        private void OnSelectExited(SelectExitEventArgs arg0)
        {
            
            // m_EditObject = null;
        }

        
        public void ToggleRun(bool isEdit)
        {
            isEditing = isEdit;
            
            Debug.Log("EditObjectProvider : ToggleEditObject : isEditing : " + isEdit);
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

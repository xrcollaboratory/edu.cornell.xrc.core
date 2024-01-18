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
    public class EditObjectProvider : MonoBehaviour, IRunnable
    {
        [SerializeField]
        private XRBaseInteractor m_Interactor;

        [SerializeField]
        private bool m_SnapBack = true;

        [SerializeField]
        private bool m_StartEditOnSet = true;

        [SerializeField]
        private InputActionProperty m_SetEditObject;

        private GameObject m_EditObject;
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
        /// The game object that is being edited. This is provided by the interactor.
        /// </summary>
        public GameObject editObject => m_EditObject;

        private void Start()
        {
            m_EditTool = GetComponent<IEditTool>();
            m_SetEditObject.action.performed += _ => ToggleRun();
        }

        private void OnEnable()
        {
            m_SetEditObject.action.Enable();
            m_Interactor.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            m_SetEditObject.action.Disable();
            m_Interactor.selectEntered.RemoveListener(OnSelectEntered);
        }

        /// <summary>
        /// Boolean indicating whether 
        /// </summary>
        public bool isRunning => m_IsRunning;

        public void StartRun()
        {
            if (m_Interactor.hasSelection)
            {
                m_IsRunning = true;

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

        public void StopRun()
        {
            m_IsRunning = false;
            
            ((XRGrabInteractable)m_Interactable).enabled = true;

            m_EditObject = null;
            if (m_StartEditOnSet)
            {
                m_EditTool.StopRun();
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            var interactable = args.interactableObject;
            m_InitialPosition = interactable.transform.position;
            m_InitialRotation = interactable.transform.rotation;
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

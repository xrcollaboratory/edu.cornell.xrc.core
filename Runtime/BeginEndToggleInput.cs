using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using XRC.Core;

[System.Serializable]
[RequireComponent(typeof(IBeginEndToggle))]
public class BeginEndToggleInput : MonoBehaviour
{
    // [SerializeReference]
    // public List<IBeginEndToggle> m_Toggles;

    [SerializeReference]
    List<IBeginEndToggle> m_Toggles = new List<IBeginEndToggle>(); // VALID
    private IBeginEndToggle m_Toggle;
    
    [SerializeField]
    private InputActionProperty m_BeginAction;

    [SerializeField]
    private InputActionProperty m_EndAction;

    [SerializeField]
    private InputActionProperty m_ToggleAction;


    void Start()
    {
        // if (m_Toggles.Count==0)
        // {
            // m_Toggles = GetComponents<IBeginEndToggle>().ToList();
        // }

        /*m_Toggle = GetComponent<IBeginEndToggle>();
        m_ToggleAction.action.started += _ => m_Toggle.Toggle();
        m_BeginAction.action.started += _ => m_Toggle.Begin();
        m_EndAction.action.started += _ => m_Toggle.End();*/
        //
        foreach (var toggle in m_Toggles)
        {
            m_ToggleAction.action.started += _ => toggle.Toggle();
            m_BeginAction.action.started += _ => toggle.Begin();
            m_EndAction.action.started += _ => toggle.End();
        }

        // m_ToggleAction.action.started += _ => m_BeginEndToggle.Toggle();
        // m_BeginAction.action.started += _ => m_BeginEndToggle.Begin();
        // m_EndAction.action.started += _ => m_BeginEndToggle.End();
    }
    
    private void OnEnable()
    {
        m_BeginAction.action.Enable();
        m_EndAction.action.Enable();
        m_ToggleAction.action.Enable();
    }

    private void OnDisable()
    {
        m_BeginAction.action.Disable();
        m_EndAction.action.Disable();
        m_ToggleAction.action.Disable();
    }
}

using System;
using UnityEngine;

namespace XRC.Core
{
    /// <summary>
    ///     The code defines a class named UtilityOnDestroyHandler that inherits from MonoBehaviour, which means it can be
    ///     attached to a GameObject in Unity.
    ///     The class has two member variables:
    ///     m_isHandlingDisabled: A boolean flag that indicates whether the handling is disabled. It is initially set to false.
    ///     m_onDestroy: An Action delegate that represents the action to be executed on destroy. It is initially set to null.
    ///     The class has a method named DestroyWithoutHandling that can be called to destroy the object without executing the
    ///     destroy action. It performs the following steps:
    ///     Sets the m_isHandlingDisabled flag to true, indicating that handling is disabled.
    ///     Sets the m_onDestroy action to null, effectively clearing the destroy action.
    ///     Calls Destroy(this) to destroy the object.
    ///     The class also has the OnDestroy method, which is a Unity callback that gets triggered when the object is being
    ///     destroyed. It performs the following steps:
    ///     Checks if the m_onDestroy action is set to a non-null value.
    ///     If the m_onDestroy action is not null, it invokes the action, executing the destroy logic.
    ///     If the m_onDestroy action is null and the m_isHandlingDisabled flag is false, it logs an error message to the Unity
    ///     console using Debug.LogError. This error message indicates that the destroy handler was not set properly.
    ///     Overall, this code provides a utility class that allows you to handle object destruction with a custom action. It
    ///     provides a way to disable the destroy handling temporarily and also handles cases where the destroy action is not
    ///     set properly, logging an error in such situations.
    /// </summary>
    public class UtilityOnDestroyHandler : MonoBehaviour
    {
        // Action to be executed on destroy
        public Action m_onDestroy;

        // Flag to indicate if handling is disabled
        private bool m_isHandlingDisabled;

        // Unity callback when the object is being destroyed
        private void OnDestroy()
        {
            // Check if the destroy action is set
            if (m_onDestroy != null)
                // Invoke the destroy action
                m_onDestroy();
            // If the destroy action is not set and handling is not disabled
            else if (!m_isHandlingDisabled)
                // Log an error indicating that the destroy handler was not set
                Debug.LogError("UtilityOnDestroyHandler: OnDestroy: destroy handler was not set!");
        }

        // Method to destroy the object without executing the destroy action
        public void DestroyWithoutHandling()
        {
            // Disable handling
            m_isHandlingDisabled = true;

            // Clear the destroy action
            m_onDestroy = null;

            // Destroy the object
            Destroy(this);
        }
    }
}
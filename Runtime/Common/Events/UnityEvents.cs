
namespace XRC.Common.Events
{
    using UnityEngine.Events;

    /// <summary>
    /// (bool) UnityEvent subclass so the event will appear in the inspector.
    /// </summary>
    [System.Serializable] public class UnityBoolEvent : UnityEvent<bool> { }
}
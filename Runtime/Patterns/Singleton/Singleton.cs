using UnityEngine;

/// <summary>
///     MonoBehaviourSingleton is a generic base class for creating MonoBehaviour singletons. The generic type T
///     represents the type of the singleton component. The class provides a static property Instance that allows access to
///     the singleton instance.
///     The Instance property utilizes lazy initialization to create and return the singleton instance.If
///     the _instance variable is null, it searches for objects of
///     type T in the scene using
///     FindObjectsOfType.If there is only one instance found, it assigns it to _instance.If there are more than
///     one instances found, it logs an error.If no instance is found, it creates a
///     new GameObject and adds the component T to it.The new GameObject is set to be
///     hidden and not saved in the scene.Finally, it assigns the component to _instance and returns it.
///     The purpose of this
///     class is to ensure that only one instance of the specified component type exists in the scene at any given time.
/// </summary>
/// <typeparam name="T">The type of the singleton component.</typeparam>
public class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;

    /// <summary>
    ///     Gets the instance of the singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find all objects of type T in the scene
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0)
                    _instance = objs[0];
                if (objs.Length > 1)
                    // Error if more than one instance of T is found in the scene
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                if (_instance == null)
                {
                    var obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}


public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
    where T : Component
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
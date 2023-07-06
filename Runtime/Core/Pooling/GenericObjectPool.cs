namespace XRC.Common.Pooling
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object pooling solution for non UnityObject classes.
    /// </summary>
    public static class GenericObjectPool
    {
        private static Dictionary<Type, object> s_GenericPool = new Dictionary<Type, object>();

        /// <summary>
        /// Get a pooled object of the specified type using a generic ObjectPool.
        /// </summary>
        /// <typeparam name="T">The type of object to get.</typeparam>
        /// <returns>A pooled object of type T.</returns>
        public static T Get<T>()
        {
            object value;
            if (s_GenericPool.TryGetValue(typeof(T), out value)) {
                var pooledObjects = value as Stack<T>;
                if (pooledObjects.Count > 0) {
                    return pooledObjects.Pop();
                }
            }

            if (typeof(T).IsArray) {
                return (T)Activator.CreateInstance(typeof(T), new object[] { 0 });
            }
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Return the object back to the generic object pool.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="obj">The object to return.</param>
        public static void Return<T>(T obj)
        {
            object value;
            if (s_GenericPool.TryGetValue(typeof(T), out value)) {
                var pooledObjects = value as Stack<T>;
                pooledObjects.Push(obj);
            } else {
                var pooledObjects = new Stack<T>();
                pooledObjects.Push(obj);
                s_GenericPool.Add(typeof(T), pooledObjects);
            }
        }
    }
}
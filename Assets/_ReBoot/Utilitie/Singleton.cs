using System;
using System.ComponentModel;
using UnityEngine;
using Component = UnityEngine.Component;

namespace _ReBoot.Utilities
{
    /// <summary>
    /// Singleton class that is not persistent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if(Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    /// <summary>
    /// Singleton class that is persistent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonPersistent<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if(Instance == null)
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
}
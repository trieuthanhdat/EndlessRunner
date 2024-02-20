
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private static T s_Instance;

    public static bool s_IsCreated = false;

    /// <summary>
    /// singleton property
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!Application.isPlaying)
            {
                return null;
                Debug.LogError("Dont call Singleton when not play");
            }
            if (s_IsCreated)
                return s_Instance;

            if (s_Instance == null)
            {
                s_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (s_Instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    GameObject.DontDestroyOnLoad(gameObject);

                    s_Instance = gameObject.AddComponent(typeof(T)) as T;
                }

                s_IsCreated = true;
            }

            return s_Instance;
        }
    }
    public static T instance
    {
        get
        {
            if (!Application.isPlaying)
            {
                return null;
            }
            if (s_IsCreated)
                return s_Instance;

            if (s_Instance == null)
            {
                s_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (s_Instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    GameObject.DontDestroyOnLoad(gameObject);

                    s_Instance = gameObject.AddComponent(typeof(T)) as T;
                }

                s_IsCreated = true;
            }

            return s_Instance;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (s_Instance)
            Destroy(s_Instance);

        s_Instance = null;
        s_IsCreated = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{

	static T m_instance;

	static bool isQuitting = false;

	void OnApplicationQuit()
	{
		isQuitting = true;
	}

	public static T Instance 
	{
		get 
		{
			if (m_instance == null && !isQuitting)
			{
				Debug.Log("CREATED INSTANCE");
				GameObject aux = new GameObject( $"{typeof(T).ToString()}");
				m_instance = aux.AddComponent<T>();
			}

			return m_instance;
		}

		private set
		{
			m_instance = value;
		} 
	}
    /// <summary>
    /// Si quieres que no se destruya, tienes que ponerlo en el Awake() de la clase derivada. Añade el DontDestroyOnLoad(gameObject); debajo de base.Awake(); para que no se destruya al cambiar de escena.
    /// </summary>
    public virtual void Awake()
	{
		InitializeInstance();
	}

	void InitializeInstance()
	{
		if (m_instance == null)
			m_instance = (T)this;
		else if (m_instance != this)
		{
			Debug.LogWarning("DUPLICATED INSTANCE: " + gameObject.name);
			Destroy(gameObject);
		}
	}
	
	protected virtual void OnDestroy()
	{
	}
	
	public static bool HasInstance()
    {
		return m_instance != null;
    }


    public void NullifyInstance()
    {
        Instance = null;
    }
}

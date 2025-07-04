#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/*******************************************************************************
The content of this file includes portions of the proprietary AUDIOKINETIC Wwise
Technology released in source code form as part of the game integration package.
The content of this file may not be used without valid licenses to the
AUDIOKINETIC Wwise Technology.
Note that the use of the game engine is subject to the Unity(R) Terms of
Service at https://unity3d.com/legal/terms-of-service
 
License Usage
 
Licensees holding valid licenses to the AUDIOKINETIC Wwise Technology may use
this file in accordance with the end user license agreement provided with the
software or, alternatively, in accordance with the terms contained
in a written agreement between you and Audiokinetic Inc.
Copyright (c) 2025 Audiokinetic Inc.
*******************************************************************************/

[UnityEngine.AddComponentMenu("Wwise/AkAudioListener")]
[UnityEngine.RequireComponent(typeof(AkGameObj))]
[UnityEngine.DisallowMultipleComponent]
[UnityEngine.DefaultExecutionOrder(-50)]
///@brief Add this script on the game object that represent a listener.  This is normally added to the Camera object or the Player object, but can be added to any game object when implementing 3D busses.  \c isDefaultListener determines whether the game object will be considered a default listener - a listener that automatically listens to all game objects that do not have listeners attached to their AkGameObjListenerList's.
/// \sa
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__listeners.html" target="_blank">Integrating Listeners</a> (Note: This is described in the Wwise SDK documentation.)
public class AkAudioListener : UnityEngine.MonoBehaviour
{
	private static readonly DefaultListenerList defaultListeners = new DefaultListenerList();
	private ulong akGameObjectID = AkSoundEngine.AK_INVALID_GAME_OBJECT;
	private System.Collections.Generic.List<AkGameObj> EmittersToStartListeningTo = 
		new System.Collections.Generic.List<AkGameObj>();
	private System.Collections.Generic.List<AkGameObj> EmittersToStopListeningTo = 
		new System.Collections.Generic.List<AkGameObj>();

	public bool isDefaultListener = true;
	
	[UnityEngine.SerializeField]
	public bool bOverrideScalingFactor = false;
	
	[UnityEngine.SerializeField]
	private float scalingFactor = -1f;
	
	public float ScalingFactor
	{
		get
		{
			if (bOverrideScalingFactor)
			{
				return scalingFactor;
			}
			var settings = AkWwiseInitializationSettings.Instance;
			if (settings)
			{
				return settings.UserSettings.m_DefaultListenerScalingFactor;
			}

			return 1.0f;
		}
		set
		{
			if (value < 0)
			{
				scalingFactor = 0;
			}
			else
			{
				scalingFactor = value;
			}
		}
	}

	public static DefaultListenerList DefaultListeners
	{
		get { return defaultListeners; }
	}

	public void StartListeningToEmitter(AkGameObj emitter)
	{
		EmittersToStartListeningTo.Add(emitter);
		EmittersToStopListeningTo.Remove(emitter);
	}

	public void StopListeningToEmitter(AkGameObj emitter)
	{
		EmittersToStartListeningTo.Remove(emitter);
		EmittersToStopListeningTo.Add(emitter);
	}

	public void SetIsDefaultListener(bool isDefault)
	{
		if (isDefaultListener != isDefault)
		{
			isDefaultListener = isDefault;

			if (isDefault)
			{
				DefaultListeners.Add(this);
			}
			else
			{
				DefaultListeners.Remove(this);
			}
		}
	}

	private void Awake()
	{
		var akGameObj = GetComponent<AkGameObj>();
		UnityEngine.Debug.Assert(akGameObj != null);
		if (akGameObj)
		{
			akGameObj.Register();
		}

		akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
	}

	private void OnEnable()
	{
		if (isDefaultListener)
		{
			DefaultListeners.Add(this);
		}
		if (scalingFactor < 0f)
		{
			var initializer = FindObjectOfType<AkInitializer>();
			if (initializer)
			{
				scalingFactor = initializer.GetComponent<AkInitializer>().InitializationSettings.UserSettings.m_DefaultListenerScalingFactor;
			}
			else
			{
				scalingFactor = 1f;
			}
		}

		AkSoundEngine.SetScalingFactor(gameObject, ScalingFactor);
	}

	private void OnDisable()
	{
		if (isDefaultListener)
		{
			DefaultListeners.Remove(this);
		}
	}

	private void OnDestroy()
	{
		AkSoundEngine.UnregisterGameObj(gameObject);
	}

	private void Update()
	{
		for (var i = 0; i < EmittersToStartListeningTo.Count; ++i)
		{
			EmittersToStartListeningTo[i].AddListener(this);
		}
		EmittersToStartListeningTo.Clear();

		for (var i = 0; i < EmittersToStopListeningTo.Count; ++i)
		{
			EmittersToStopListeningTo[i].RemoveListener(this);
		}
		EmittersToStopListeningTo.Clear();
	}

	public ulong GetAkGameObjectID()
	{
		return akGameObjectID;
	}

	public class BaseListenerList
	{
		// @todo: Use HashSet<ulong> and CopyTo() with a private ulong[]
		private readonly System.Collections.Generic.List<ulong> listenerIdList = new System.Collections.Generic.List<ulong>();

		private readonly System.Collections.Generic.List<AkAudioListener> listenerList =
			new System.Collections.Generic.List<AkAudioListener>();

		public System.Collections.Generic.List<AkAudioListener> ListenerList
		{
			get { return listenerList; }
		}

		/// <summary>
		///     Uniquely adds listeners to the list
		/// </summary>
		/// <param name="listener"></param>
		/// <returns></returns>
		public virtual bool Add(AkAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}

			var gameObjectId = listener.GetAkGameObjectID();
			if (listenerIdList.Contains(gameObjectId))
			{
				return false;
			}

			listenerIdList.Add(gameObjectId);
			listenerList.Add(listener);
			return true;
		}

		/// <summary>
		///     Removes listeners from the list
		/// </summary>
		/// <param name="listener"></param>
		/// <returns></returns>
		public virtual bool Remove(AkAudioListener listener)
		{
			if (listener == null)
			{
				return false;
			}

			var gameObjectId = listener.GetAkGameObjectID();
			if (!listenerIdList.Remove(gameObjectId))
			{
				return false;
			}

			listenerList.Remove(listener);
			return true;
		}

		public ulong[] GetListenerIds()
		{
			return listenerIdList.ToArray();
		}
	}

	public class DefaultListenerList : BaseListenerList
	{
		public override bool Add(AkAudioListener listener)
		{
			var ret = base.Add(listener);
			if (ret && AkSoundEngine.IsInitialized())
			{
				AkSoundEngine.AddDefaultListener(listener.gameObject);
			}
			return ret;
		}

		public override bool Remove(AkAudioListener listener)
		{
			var ret = base.Remove(listener);
			if (ret && AkSoundEngine.IsInitialized())
			{
				AkSoundEngine.RemoveDefaultListener(listener.gameObject);
			}

			return ret;
		}
	}

	#region WwiseMigration

#pragma warning disable 0414 // private field assigned but not used.

	[UnityEngine.SerializeField]
	// Wwise v2016.2 and below supported up to 8 listeners[0-7].
	public int listenerId = 0;

#pragma warning restore 0414 // private field assigned but not used.

	public void Migrate14()
	{
		var wasDefaultListener = listenerId == 0;
		UnityEngine.Debug.Log("WwiseUnity: AkAudioListener.Migrate14 for " + gameObject.name);
		isDefaultListener = wasDefaultListener;
	}

	#endregion
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
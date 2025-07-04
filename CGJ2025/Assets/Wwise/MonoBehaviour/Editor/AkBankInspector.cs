#if UNITY_EDITOR
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

[UnityEditor.CanEditMultipleObjects]
[UnityEditor.CustomEditor(typeof(AkBank), true)]
public class AkBankInspector : AkBaseInspector
{
	private readonly AkUnityEventHandlerInspector m_LoadBankEventHandlerInspector = new AkUnityEventHandlerInspector();
	private readonly AkUnityEventHandlerInspector m_UnloadBankEventHandlerInspector = new AkUnityEventHandlerInspector();

#if !(AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES)
	private UnityEditor.SerializedProperty loadAsync;
	private UnityEditor.SerializedProperty decode;
	private UnityEditor.SerializedProperty saveDecoded;
#endif

	private void OnEnable()
	{
		m_LoadBankEventHandlerInspector.Init(serializedObject, "triggerList", "Load On: ", false);
		m_UnloadBankEventHandlerInspector.Init(serializedObject, "unloadTriggerList", "Unload On: ", false);

#if !(AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES)
		loadAsync = serializedObject.FindProperty("loadAsynchronous");
		decode = serializedObject.FindProperty("decodeBank");
		saveDecoded = serializedObject.FindProperty("saveDecodedBank");
#endif
	}

	public override void OnChildInspectorGUI()
	{
		m_LoadBankEventHandlerInspector.OnGUI();
		m_UnloadBankEventHandlerInspector.OnGUI();
#if !(AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES)
		using (new UnityEditor.EditorGUILayout.VerticalScope("box"))
		{
			UnityEditor.EditorGUILayout.PropertyField(loadAsync, new UnityEngine.GUIContent("Asynchronous:"));
			UnityEditor.EditorGUILayout.PropertyField(decode, new UnityEngine.GUIContent("Decode compressed data:"));

			if (!decode.boolValue)
				return;

			var oldSaveDecodedValue = saveDecoded.boolValue;
			UnityEditor.EditorGUILayout.PropertyField(saveDecoded, new UnityEngine.GUIContent("Save decoded bank:"));
			if (!oldSaveDecodedValue || saveDecoded.boolValue)
				return;

			var bank = target as AkBank;
			var decodedBankPath = System.IO.Path.Combine(AkBasePathGetter.Get().DecodedBankFullPath, bank.data.Name + ".bnk");

			try
			{
				System.IO.File.Delete(decodedBankPath);
			}
			catch (System.Exception e)
			{
				UnityEngine.Debug.Log("WwiseUnity: Could not delete existing decoded SoundBank. Please delete it manually. " + e);
			}
		}
#endif
	}
}
#endif

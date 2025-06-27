using System;
using Cysharp.Threading.Tasks;
using ParadoxNotion;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

public class CustomSceneManager
{
    // ReSharper disable Unity.PerformanceAnalysis
    public static async UniTask LoadSceneAsychonized(string sceneName, System.Action<AsyncOperation> loadSceneCompleted = null, LoadSceneMode mode = LoadSceneMode.Single)
    {
        // 加载场景
        var operation = SceneManager.LoadSceneAsync(sceneName, mode);
        await operation.ToUniTask();
        // 调用完成回调
        loadSceneCompleted?.Invoke(operation);
    }

    public static async UniTask UnloadScene(string sceneName, Action<AsyncOperation> LoadSceneCompleted = null)
    {
        var operation  =  SceneManager.UnloadSceneAsync(sceneName);
        await operation.ToUniTask();
    }
}
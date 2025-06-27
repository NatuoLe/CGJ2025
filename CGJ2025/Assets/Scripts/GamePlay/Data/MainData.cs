using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.DialogueTrees;
using UnityEngine;
using ThGold.Common;

public class MainData : MonoSingleton<MainData>
{
    private void Awake()
    {
        
    }


    // Flag to track if loading is complete
    public bool IsDatabaseLoaded { get; private set; }

    async UniTask Start()
    {
    }

    // Static initialization method
    public async UniTask InitConfig()
    {
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NodeCanvas.Tasks.Actions;
using Sirenix.OdinInspector;
using ThGold.Common;
using ThGold.Event;
using ThGold.Wwise;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    

    private void Awake()
    {
        InitCombatLogic();
    }

    private void InitCombatLogic()
    {
        WwiseController.Instance.PlayBGM("BGM");
    }

    private void Update()
    {
      
    }
    

}
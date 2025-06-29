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
using EventHandler = ThGold.Event.EventHandler;

public class GameManager : MonoSingleton<GameManager>
{
    [Tooltip("是否暂停")] public bool stop;
    [Tooltip("当前时间")] public float curTime;
    [Tooltip("最大时间")] public float MaxTime;
    [Tooltip("速率")] public float rating;

    [ShowInInspector] public Dictionary<BlockType, SystemBlock> Blocks;

    private void Awake()
    {
        InitCombatLogic();
    }

    private void InitCombatLogic()
    {
        Blocks = new Dictionary<BlockType, SystemBlock>();

        WwiseController.Instance.PlayBGM("BGM");
        //EventHandler.Instance.EventDispatcher.AddEventListener(CustomEvent.CardUpgradeDone, updateData);
    }

    private void updateData(IEvent ievent)
    {
    }

    public void UpdateData()
    {
        updateData(null);
    }
}
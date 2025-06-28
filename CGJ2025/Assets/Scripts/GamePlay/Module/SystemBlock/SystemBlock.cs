using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SystemBlock:MonoBehaviour
{
    [Tooltip("产出类型")] public BonuesType product;
    [Tooltip("产出个数")] public int productCount;
    [Tooltip("产出速率")] public float curRating;
    [Tooltip("产出时间")] public float outPutTime;
    [Tooltip("当前污染")] public float curEmissionCount;
    [Tooltip("卡牌列表")]
    public List<SystemCard> cards;

    public void Awake()
    {
        productCount = 1;
        curRating = 1f;
        curEmissionCount = 0;
    }

    public void Update()
    {
        UpdateTime();
    }

    public virtual void UpdateTime()
    {
        if (!GameManager.Instance.stop)
        {
            curRating += Time.deltaTime * outPutTime;
        }
    }

    public void AddRating(float rate)
    {
        curRating += rate;
    }

    public void UpdateSystemBlock()
    {
        foreach (var card in cards)
        {
            
        }
    }
}
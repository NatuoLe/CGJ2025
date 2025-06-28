using System.Collections.Generic;
using ThGold.Event;
using UnityEngine;

public struct CardData
{
    public int CardID;
    //data
    public int EmissionAmount;
    public float BonuesRating;
    public CardBuff Buff;
    //cost
    public List<int> UpgradeCost;
    public float UpgradeTime;
    public int NextId;
}
public class SystemCard
{
    public int CardID;
    public CardData data;
    public void Init()
    {
        
    }
    public void CardUpgradeDone()
    {
        Debug.Log("升级卡牌完成");
        UpdateCard(data.NextId);
        EventHandler.Instance.EventDispatcher.DispatchEvent(CustomEvent.CardUpgradeDone);
    }

    public void UpdateCard(int cardId)
    {
        Debug.Log("更新卡牌 " + cardId);
    }
    public void UpdateCard(CardData cardData)
    {
        Debug.Log("更新卡牌 " + cardData);
    }

}
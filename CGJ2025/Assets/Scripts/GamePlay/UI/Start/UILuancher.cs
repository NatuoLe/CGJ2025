using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILuancher : MonoBehaviour
{
    [SerializeField] private Image Image_Fill;
    [SerializeField] private Text Text_Tip;

    public void ChangeBar(string text, float startValue, float endValue)
    {
        Text_Tip.text = text;
        Image_Fill.DOFillAmount(endValue, 0.3f).From(startValue);
    }
}
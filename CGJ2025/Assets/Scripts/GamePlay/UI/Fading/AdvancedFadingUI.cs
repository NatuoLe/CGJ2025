using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class AdvancedFadingUI : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float showDuration = 1f;
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void Show(string content)
    {
        text.text = content;
        // 如果已经有淡入淡出在进行，先停止
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        
        fadeRoutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // 淡入效果
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeInDuration;
            canvasGroup.alpha = fadeCurve.Evaluate(progress);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        // 保持显示状态
        yield return new WaitForSeconds(showDuration);
        
        // 淡出效果
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeOutDuration;
            canvasGroup.alpha = 1 - fadeCurve.Evaluate(progress);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        
        fadeRoutine = null;
    }

    public void HideImmediately()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }
        canvasGroup.alpha = 0f;
    }
}
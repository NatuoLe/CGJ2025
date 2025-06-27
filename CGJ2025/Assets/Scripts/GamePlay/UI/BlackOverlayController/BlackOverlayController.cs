using UnityEngine;
using UnityEngine.UI;

public class BlackOverlayController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;  
    [SerializeField] private float displayDuration = 5f;    
    [SerializeField] private float fadeOutDuration = 0.5f; 
    
    private CanvasGroup canvasGroup;
    private bool isActive = false;
    private float timer = 0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; 
        gameObject.SetActive(false); 
    }
    
    public void StartBlackOverlay()
    {
        gameObject.SetActive(true);
        isActive = true;
        timer = 0f;
    }

    private void Update()
    {
        if (!isActive) return;
        
        timer += Time.deltaTime;
        

        if (timer <= fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
        }

        else if (timer <= fadeInDuration + displayDuration)
        {
            canvasGroup.alpha = 1f;
        }

        /*else if (timer <= fadeInDuration + displayDuration + fadeOutDuration)
        {
            float fadeOutProgress = (timer - fadeInDuration - displayDuration) / fadeOutDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeOutProgress);
        }
        else
        {
            canvasGroup.alpha = 0f;
            isActive = false;
            gameObject.SetActive(false);
            OnOverlayComplete();
        }*/
    }
    
    protected virtual void OnOverlayComplete()
    {
        Debug.Log("黑幕效果完成");
    }
}
using UnityEngine;
using TMPro;

public class HintHandler : MonoBehaviour
{
    private CanvasGroup cg;
    private TextMeshProUGUI hintText;
    private float lifetime;
    
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        hintText = GetComponentInChildren<TextMeshProUGUI>();
        lifetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
            cg.alpha = Mathf.Clamp01(lifetime / 0.6f);
        }
    }

    public void ShowHint(string text, float duration)
    {
        hintText.text = text;
        lifetime = duration;
        // play hint sound
    }
}

using UnityEngine;
using TMPro;

public class TimerHandler : MonoBehaviour
{
    private TextMeshProUGUI textbox;
    [HideInInspector] public float time;
    [HideInInspector] public bool paused;

    private void Awake()
    {
        textbox = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (paused) return;
        // decrease normally, increase in REVERSE
        int sign = LevelManager.instance.IsReverse() ? +1 : -1;
        time += sign * Time.deltaTime;
        int itime = (int)time;
        textbox.text = $"{itime / 60:00}:{itime % 60:00}";
    }
}

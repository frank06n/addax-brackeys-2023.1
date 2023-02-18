using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectivesHandler : MonoBehaviour
{
    private bool animating;
    private bool visible;
    private RectTransform rt;
    private TaskItem[] taskItems;
    [SerializeField] private Sprite spr_Unchecked;
    [SerializeField] private Sprite spr_Checked;

    void Awake()
    {
        rt = (RectTransform)transform;
        animating = false;
        visible = false;

        taskItems = new TaskItem[4];
        for (int i=0; i<4; i++)
        {
            taskItems[i] = new TaskItem(transform.GetChild(i+1));
        }
    }

    public void Toggle()
    {
        if (animating) return;
        StartCoroutine(Animate(!visible));
    }

    private IEnumerator Animate(bool show)
    {
        animating = true;
        float animProgress = 0f;

        const float ANIM_DURATION = 0.25f;
        float PANEL_Y = rt.anchoredPosition.y;

        while (animProgress <= 1f)
        {
            float x = show ? Mathf.Lerp(300, -10, animProgress) : Mathf.Lerp(-10, 300, animProgress);
            rt.anchoredPosition = new Vector2(x, PANEL_Y);
            animProgress += Time.deltaTime / ANIM_DURATION;
            yield return null;
        }
        animating = false;
        visible = show;
    }
    
    public void InitTasks(string[] tasks, bool check)
    {
        int L = tasks.Length;
        for (int i=0; i<4; i++)
        {
            if (i >= L)
                taskItems[i].textbox.gameObject.SetActive(false);
            else
            {
                taskItems[i].textbox.text = tasks[i];
                taskItems[i].checkbox.sprite = check ? spr_Checked : spr_Unchecked;
            }
        }
    }

    public bool IsTaskChecked(int ix)
    {
        return taskItems[ix].checkbox.sprite == spr_Checked;
    }

    public void CheckTask(int ix, bool check)
    {
        taskItems[ix].checkbox.sprite = check ? spr_Checked : spr_Unchecked;
        if (!visible) Toggle();
    }

    private class TaskItem
    {
        public TextMeshProUGUI textbox;
        public Image checkbox;
        public TaskItem(Transform t)
        {
            textbox = t.GetComponent<TextMeshProUGUI>();
            checkbox = t.GetComponentInChildren<Image>();
        }
    }
}
